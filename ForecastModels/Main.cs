// <copyright file="Main.cs" company="AlinaP">
// Copyright (c) AlinaP. All rights reserved.
// </copyright>

namespace TelegramBotWeather.Models
{
    /// <summary>
    /// Represents the main weather parameters, including temperature and perceived temperature.
    /// </summary>
    public class Main
    {
        /// <summary>
        /// Gets or sets the current temperature in the specified location.
        /// </summary>
        public float Temp { get; set; }

        /// <summary>
        /// Gets or sets the perceived temperature, which is how the weather "feels" to a person.
        /// </summary>
        public float Feels_like { get; set; }
    }
}