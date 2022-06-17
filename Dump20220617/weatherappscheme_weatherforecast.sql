-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: localhost    Database: weatherappscheme
-- ------------------------------------------------------
-- Server version	8.0.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `weatherforecast`
--

DROP TABLE IF EXISTS `weatherforecast`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `weatherforecast` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CityId` int(11) NOT NULL,
  `Date` datetime NOT NULL,
  `TemperatureC` int(11) NOT NULL,
  `Summary` char(50) NOT NULL,
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `weatherforecast`
--

LOCK TABLES `weatherforecast` WRITE;
/*!40000 ALTER TABLE `weatherforecast` DISABLE KEYS */;
INSERT INTO `weatherforecast` VALUES (1,23,'2022-06-18 00:00:00',3,'Sunny'),(2,23,'2022-06-19 00:00:00',27,'Rainy'),(3,23,'2022-06-20 00:00:00',-19,'Sunny'),(4,23,'2022-06-21 00:00:00',-19,'Hot'),(5,23,'2022-06-22 00:00:00',6,'Rainy'),(6,23,'2022-06-23 00:00:00',-13,'Snowy'),(7,23,'2022-06-24 00:00:00',19,'Cold'),(8,23,'2022-06-25 00:00:00',-12,'Cold'),(9,23,'2022-06-26 00:00:00',-21,'Cloudy'),(10,23,'2022-06-27 00:00:00',-24,'Rainy'),(11,78,'2022-06-18 00:00:00',-14,'Sunny'),(12,78,'2022-06-19 00:00:00',-18,'Cold'),(13,78,'2022-06-20 00:00:00',7,'Hot'),(14,78,'2022-06-21 00:00:00',-21,'Snowy'),(15,78,'2022-06-22 00:00:00',-11,'Rainy'),(16,78,'2022-06-23 00:00:00',3,'Windy'),(17,78,'2022-06-24 00:00:00',14,'Snowy'),(18,78,'2022-06-25 00:00:00',17,'Hot'),(19,78,'2022-06-26 00:00:00',-15,'Sunny'),(20,78,'2022-06-27 00:00:00',21,'Rainy'),(21,116,'2022-06-18 00:00:00',-7,'Sunny'),(22,116,'2022-06-19 00:00:00',-3,'Windy'),(23,116,'2022-06-20 00:00:00',-24,'Sunny'),(24,116,'2022-06-21 00:00:00',-5,'Hot'),(25,116,'2022-06-22 00:00:00',23,'Cold'),(26,116,'2022-06-23 00:00:00',11,'Snowy'),(27,116,'2022-06-24 00:00:00',-18,'Rainy'),(28,116,'2022-06-25 00:00:00',-10,'Hot'),(29,116,'2022-06-26 00:00:00',6,'Rainy'),(30,116,'2022-06-27 00:00:00',-3,'Sunny');
/*!40000 ALTER TABLE `weatherforecast` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-06-17  0:06:27
