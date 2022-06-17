DROP TABLE weatherappscheme.City;
DROP TABLE weatherappscheme.WeatherForecast;

CREATE TABLE weatherappscheme.City
(
Id int not null unique,
Name char(50) not null
);

CREATE TABLE weatherappscheme.WeatherForecast
(
Id int not null unique AUTO_INCREMENT,
CityId int not null,
Date DateTime not null,
TemperatureC int not null,
Summary char(50) not null
);