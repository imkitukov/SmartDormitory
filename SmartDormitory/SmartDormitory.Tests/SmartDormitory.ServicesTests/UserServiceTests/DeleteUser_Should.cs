﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartDormitory.App.Data;
using SmartDormitory.Data.Models;
using SmartDormitory.Services;
using SmartDormitory.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDormitory.Tests.SmartDormitory.AppTests.UserServiceTests
{
	[TestClass]
	public class DeleteUser_Should
	{
		private DbContextOptions<SmartDormitoryContext> contextOptions;
		private Mock<UserManager<User>> userManagerMock;
		private Mock<RoleManager<IdentityRole>> roleManagerMock;
		private User user;

		[TestMethod]
		public async Task Throw_EntityDoesntExistException_When_User_Does_Not_Exist()
		{
			// Arrange
			contextOptions = new DbContextOptionsBuilder<SmartDormitoryContext>()
			.UseInMemoryDatabase(databaseName: "DeleteUser_Throw_EntityDoesntExistException_When_User_Does_Not_Exist")
				.Options;

			string userId = Guid.NewGuid().ToString();

			userManagerMock = MockUserManager<User>();

			roleManagerMock = MockRoleManager();

			// Act && Assert
			using (var assertContext = new SmartDormitoryContext(contextOptions))
			{
				var userService = new UserService(assertContext, userManagerMock.Object, roleManagerMock.Object);

				await Assert.ThrowsExceptionAsync<EntityDoesntExistException>(
					() => userService.DeleteUser(userId));
			}
		}


		[TestMethod]
		public async Task Remove_User_When_Exists_And_Is_Not_Deleted()
		{
			// Arrange
			contextOptions = new DbContextOptionsBuilder<SmartDormitoryContext>()
			.UseInMemoryDatabase(databaseName: "Remove_User_When_Exists_And_Is_Not_Deleted")
				.Options;

			string userId = Guid.NewGuid().ToString();

			user = new User()
			{
				Id = userId,
				UserName = "testUserName"
			};

			userManagerMock = MockUserManager<User>();

			roleManagerMock = MockRoleManager();

			using (var actContext = new SmartDormitoryContext(contextOptions))
			{
				await actContext.Users.AddAsync(user);
				await actContext.SaveChangesAsync();
			}

			// Act && Assert
			using (var assertContext = new SmartDormitoryContext(contextOptions))
			{
				var userService = new UserService(assertContext, userManagerMock.Object, roleManagerMock.Object);

				await userService.DeleteUser(userId);
				var users = await assertContext.Users.ToListAsync();
				Assert.IsTrue(users[0].IsDeleted);
			}
		}



		private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
		{
			var store = new Mock<IUserStore<TUser>>();
			var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
			mgr.Object.UserValidators.Add(new UserValidator<TUser>());
			mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

			return mgr;
		}

		private Mock<RoleManager<IdentityRole>> MockRoleManager()
		{
			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
				new Mock<IRoleStore<IdentityRole>>().Object,
				new IRoleValidator<IdentityRole>[0],
				new Mock<ILookupNormalizer>().Object,
				new Mock<IdentityErrorDescriber>().Object,
				new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

			return mockRoleManager;
		}
	}
}
