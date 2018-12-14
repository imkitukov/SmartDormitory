﻿using Microsoft.AspNetCore.Http;
using SmartDormitory.Services.Exceptions;
using System.Threading.Tasks;

namespace SmartDormitory.App.Infrastructure.Middleware
{
	public class WrongRouteMiddleware
	{
		private readonly RequestDelegate next;

		public WrongRouteMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await this.next.Invoke(context);

				if (context.Response.StatusCode == 404)
				{
					context.Response.Redirect("/404");
				}
			}
			catch (EntityDoesntExistException ex)
			{
				context.Response.Redirect("/404");
			}
			catch (InvalidClientInputException ex)
			{
				context.Response.Redirect("/404");
			}
		}
	}
}
