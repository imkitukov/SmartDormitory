﻿using Microsoft.EntityFrameworkCore;
using SmartDormitory.App.Data;
using SmartDormitory.Data.Models;
using SmartDormitory.Services.Abstract;
using SmartDormitory.Services.Contracts;
using SmartDormitory.Services.Exceptions;
using SmartDormitory.Services.Models.Sensors;
using SmartDormitory.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDormitory.Services
{
	public class SensorsService : BaseService, ISensorsService
	{
		private readonly IMeasureTypeService measureTypeService;

		public SensorsService(SmartDormitoryContext context,
			IMeasureTypeService measureTypeService)
			: base(context)
		{
			this.measureTypeService = measureTypeService;
		}

		public async Task<IEnumerable<MapSensorServiceModel>> GetAllPublicCoordinates()
			=> await this.Context
				.Sensors
				.Where(s => s.IsPublic && !s.IsDeleted)
				.Select(s => new MapSensorServiceModel
				{
					SensorType = s.IcbSensor.MeasureType.SuitableSensorType,
					Name = s.Name,
					CreatedOn = (DateTime)s.CreatedOn,
					UserId = s.UserId,
					IsPublic = s.IsPublic,
					Coordinates = new Coordinates
					{
						Latitude = s.Coordinates.Latitude,
						Longitude = s.Coordinates.Longitude
					},
				})
				.ToListAsync();

		public async Task<IEnumerable<MapSensorServiceModel>> GetAllUserPrivateCoordinates(string userId)
			=> await this.Context
						 .Sensors
						 .Where(s => !s.IsDeleted && s.UserId == userId && !s.IsPublic)
						 .Select(s => new MapSensorServiceModel
						 {
							 SensorType = s.IcbSensor.MeasureType.SuitableSensorType,
							 Name = s.Name,
							 CreatedOn = (DateTime)s.CreatedOn,
							 UserId = s.UserId,
							 IsPublic = s.IsPublic,
							 Coordinates = new Coordinates
							 {
								 Latitude = s.Coordinates.Latitude,
								 Longitude = s.Coordinates.Longitude
							 },
						 })
						 .ToListAsync();

		public async Task<IEnumerable<AdminListSensorModel>> AllAdmin(string measureTypeId = "all",
			int isPublic = -1, int alarmOn = -1, int page = 1, int pageSize = 10, string searchTerm = "")
		{
			var sensors = this.Context.Sensors
				.Where(u => !u.User.IsDeleted)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				sensors = sensors.Where(s => !s.IsDeleted &&
								(s.Name.ToLower().Contains(searchTerm.ToLower()) ||
									s.Description.ToLower().Contains(searchTerm.ToLower()) ||
									s.User.UserName.ToLower().Contains(searchTerm.ToLower())));
			}
			sensors = await QueryFilter(sensors, measureTypeId, isPublic, alarmOn);

			return await sensors
						   .OrderByDescending(s => s.CreatedOn)
						   .Skip((page - 1) * pageSize)
						   .Take(pageSize)
						   .Select(s => new AdminListSensorModel
						   {
							   Id = s.Id,
							   Name = s.Name,
							   IsDeleted = s.IsDeleted,
							   SensorType = s.IcbSensor.MeasureType.SuitableSensorType,
							   OwnerId = s.UserId,
							   OwnerUsername = s.User.UserName,
							   IsPublic = s.IsPublic,
							   AlarmOn = s.AlarmOn
						   }).ToListAsync();
		}

		public async Task DeleteSensor(string sensorId)
		{
			Validator.ValidateNull(sensorId);
			Validator.ValidateGuid(sensorId);
			var sensor = await Context.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);

			if (sensor is null)
			{
				throw new EntityDoesntExistException(nameof(Sensor).ToLower(), sensorId);
			}

			sensor.IsDeleted = !sensor.IsDeleted ? true : false;

			this.Context.Sensors.Update(sensor);
			await this.Context.SaveChangesAsync();
		}

		public async Task<string> RegisterNewSensor(string userId, string icbSensorId, string name, string description,
			int pollingInterval, bool isPublic, bool alarmOn, float minRange,
			float maxRange, double longtitude, double latitude, bool switchOn)
		{
			string returnSensorId = "";
			if (await this.Context.IcbSensors.AnyAsync(s => s.Id == icbSensorId))
			{
				var sensor = new Sensor()
				{
					MaxRangeValue = maxRange,
					MinRangeValue = minRange,
					AlarmOn = alarmOn,
					Description = description,
					Name = name,
					UserId = userId,
					IsPublic = isPublic,
					PollingInterval = pollingInterval,
					IcbSensorId = icbSensorId,
					Coordinates = new Coordinates
					{
						Latitude = latitude,
						Longitude = longtitude
					},
					CreatedOn = DateTime.Now,
					SwitchOn = switchOn,
					CurrentValue = -50
				};
				await this.Context.Sensors.AddAsync(sensor);
				await this.Context.SaveChangesAsync();
				returnSensorId = sensor.Id;
			}
			return returnSensorId;
		}

		public async Task<Sensor> GetSensorById(string sensorId)
		{
			Validator.ValidateNull(sensorId);
			Validator.ValidateGuid(sensorId);

			return await this.Context.Sensors
				.Where(s => s.Id == sensorId && !s.IsDeleted)
				.Include(icb => icb.IcbSensor)
				.ThenInclude(mt => mt.MeasureType)
				.FirstOrDefaultAsync();
		}

		public async Task<int> TotalSensorsByCriteria(string measureTypeId = "all", int isPublic = -1,
			int alarmOn = -1, string searchTerm = "")
		{
			var sensors = this.Context.Sensors.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				sensors = sensors.Where(s => !s.IsDeleted &&
								(s.Name.ToLower().Contains(searchTerm.ToLower()) ||
									s.Description.ToLower().Contains(searchTerm.ToLower()) ||
									s.User.UserName.ToLower().Contains(searchTerm.ToLower())));
			}
			sensors = await QueryFilter(sensors, measureTypeId, isPublic, alarmOn);

			return await sensors.CountAsync();
		}

		public async Task<int> TotalSensors()
			=> await this.Context.Sensors.CountAsync(s => !s.IsDeleted);

		public async Task<IEnumerable<UserSensorListModel>> GetUserSensors(string userId,
			string searchTerm = "", string measureTypeId = "all", int alarmOn = -1, int isPublic = -1)
		{
			var user = this.Context.Users.FirstOrDefault(u => u.Id == userId);
			if (user is null)
			{
				throw new EntityDoesntExistException("User does not exist");
			}

			var sensors = this.Context.Sensors
				.Where(s => s.UserId == userId && !s.IsDeleted)
				.Include(s => s.IcbSensor)
				.ThenInclude(mt => mt.MeasureType)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				sensors = sensors.Where(s => s.Name.Contains(searchTerm) ||
										   s.Description.Contains(searchTerm));
			}
			sensors = await QueryFilter(sensors, measureTypeId, isPublic, alarmOn);

			return await sensors
							 .OrderByDescending(s => s.CreatedOn)
							 .Select(s => new UserSensorListModel
							 {
								 Id = s.Id,
								 Name = s.Name,
								 SensorType = s.IcbSensor.MeasureType.SuitableSensorType,
								 MeasureUnit = s.IcbSensor.MeasureType.MeasureUnit,
								 PollingInterval = s.PollingInterval,
								 CreatedOn = (DateTime)s.CreatedOn,
								 IsPublic = s.IsPublic,
								 AlarmOn = s.AlarmOn,
								 Value = ExtractValue(s),
								 LastUpdateOn = s.LastUpdateOn
							 })
							 .ToListAsync();
		}

		private string ExtractValue(Sensor sensor)
		{
			string result = "";
			if (sensor.IcbSensor.MeasureType.SuitableSensorType == "Boolean switch (door/occupancy/etc)")
			{
				if (sensor.CurrentValue == 1)
				{
					result = "true";
				}
				else if (sensor.CurrentValue == 0)
				{
					result = "false";
				}
			}
			else
			{
				result = sensor.CurrentValue.ToString();
			}
			return result;
		}

		public async Task<GaugeDataServiceModel> GetGaugeData(string sensorId)
		{
			Validator.ValidateNull(sensorId);
			Validator.ValidateGuid(sensorId);

			var sensor = await this.Context
								   .Sensors
								   .Where(s => !s.IsDeleted && s.Id == sensorId)
								   .Select(s => new GaugeDataServiceModel
								   {
									   CurrentValue = s.CurrentValue,
									   IsUpdated = DateTime.Now.Subtract(s.LastUpdateOn)
														   .TotalSeconds < s.PollingInterval
								   })
								   .FirstOrDefaultAsync();

			return sensor;
		}

		public async Task<string> Update(string sensorId, string userId, string icbSensorId, string name,
			string description, int pollingInterval, bool isPublic, bool alarmOn, float minRange,
			float maxRange, double longtitude, double latitude, bool switchOn)
		{
			var sensor = await this.GetSensorById(sensorId);

			if (sensor == null)
			{
				throw new EntityDoesntExistException($"\nSensor is not present in the database.");
			}

			sensor.UserId = userId;
			sensor.IcbSensorId = icbSensorId;
			sensor.Name = name;
			sensor.Description = description;
			sensor.PollingInterval = pollingInterval;
			sensor.IsPublic = isPublic;
			sensor.AlarmOn = alarmOn;
			sensor.MinRangeValue = minRange;
			sensor.MaxRangeValue = maxRange;
			sensor.Coordinates.Longitude = longtitude;
			sensor.Coordinates.Latitude = latitude;
			sensor.ModifiedOn = DateTime.Now;
			sensor.SwitchOn = switchOn;

			this.Context.Sensors.Update(sensor);
			await this.Context.SaveChangesAsync();
			return sensor.Id;
		}

		public async Task<IEnumerable<MapSensorServiceModel>> GetAllCoordinates()
			=> await this.Context
				.Sensors
				.Where(s => !s.IsDeleted)
				.Select(s => new MapSensorServiceModel
				{
					Id = s.Id,
					SensorType = s.IcbSensor.MeasureType.SuitableSensorType,
					Name = s.Name,
					CreatedOn = (DateTime)s.CreatedOn,
					UserId = s.UserId,
					IsPublic = s.IsPublic,
					Value = s.CurrentValue,
					Coordinates = new Coordinates
					{
						Latitude = s.Coordinates.Latitude,
						Longitude = s.Coordinates.Longitude
					},
				})
				.ToListAsync();

		public async Task<IEnumerable<Sensor>> GetAllForUpdate()
			=> await this.Context
						 .Sensors
						 .Where(s => !s.IsDeleted &&
							DateTime.Now.Subtract(s.LastUpdateOn).TotalSeconds >= s.PollingInterval)
						 .ToListAsync();

		private async Task<IQueryable<Sensor>> QueryFilter(IQueryable<Sensor> sensors,
			string measureTypeId = "all", int isPublic = -1, int alarmOn = -1)
		{
			if (measureTypeId != "all")
			{
				if (await this.measureTypeService.Exists(measureTypeId))
				{
					sensors = sensors.Where(s => s.IcbSensor.MeasureTypeId == measureTypeId);
				}
			}
			if (isPublic != -1 && (isPublic == 0 || isPublic == 1))
			{
				bool isPublicValue = isPublic != 0;
				sensors = sensors.Where(s => s.IsPublic == isPublicValue);
			}
			if (alarmOn != -1 && (alarmOn == 0 || alarmOn == 1))
			{
				bool alarmSetValue = alarmOn != 0;
				sensors = sensors.Where(s => s.AlarmOn == alarmSetValue);
			}
			return sensors;
		}

		public async Task<IEnumerable<MapSensorServiceModel>> GetAllUserCoordinates(string userId)
			=> await this.Context
						 .Sensors
						 .Where(s => !s.IsDeleted && s.UserId == userId)
						 .Select(s => new MapSensorServiceModel
						 {
							 Id = s.Id,
							 SensorType = s.IcbSensor.MeasureType.SuitableSensorType,
							 Name = s.Name,
							 CreatedOn = (DateTime)s.CreatedOn,
							 UserId = s.UserId,
							 IsPublic = s.IsPublic,
							 Coordinates = new Coordinates
							 {
								 Latitude = s.Coordinates.Latitude,
								 Longitude = s.Coordinates.Longitude
							 },
						 })
						 .ToListAsync();
	}
}
