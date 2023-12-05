// <copyright file="Weather.cs" company="AlinaP">
// Copyright (c) AlinaP. All rights reserved.
// </copyright>

namespace TelegramBotWeather.Models
{
    /// <summary>
    /// Represents weather information for a specific location.
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// Gets or sets the unique identifier for the weather condition.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the general category of the weather condition.
        /// </summary>
        public string Main { get; set; }

        /// <summary>
        /// Gets or sets a detailed description of the weather condition.
        /// </summary>
        public string Description { get; set; }
    }
}