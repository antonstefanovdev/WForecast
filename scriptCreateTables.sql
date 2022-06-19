CREATE TABLE weatherappscheme.City
(
Id int not null unique AUTO_INCREMENT,
Name char(50) not null
);

CREATE TABLE weatherappscheme.WeatherForecast
(
Id int not null unique AUTO_INCREMENT,
CityId int not null,
Date DateTime not null,
TemperatureDayC int not null,
TemperatureNightC int not null,
WindSpeed int not null
);