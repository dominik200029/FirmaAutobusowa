-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: firmaautobusowa
-- ------------------------------------------------------
-- Server version	8.0.42

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
-- Table structure for table `bus`
--

DROP TABLE IF EXISTS `bus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bus` (
  `Registration_number` varchar(7) NOT NULL,
  `Number_of_seats` int NOT NULL,
  `Year_of_production` year NOT NULL,
  `Mark` varchar(25) NOT NULL,
  `Model` varchar(25) DEFAULT NULL,
  `Date_of_next_inspection` date NOT NULL,
  PRIMARY KEY (`Registration_number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bus`
--

LOCK TABLES `bus` WRITE;
/*!40000 ALTER TABLE `bus` DISABLE KEYS */;
INSERT INTO `bus` VALUES ('KR10001',2,2015,'marka','modell','2025-05-02'),('KR11111',5,2010,'Marka','Model','2025-07-13'),('WR452HJ',5,2005,'marka','C4','2025-05-02');
/*!40000 ALTER TABLE `bus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `Client_ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Address` varchar(50) DEFAULT NULL,
  `Phone_number` int NOT NULL,
  PRIMARY KEY (`Client_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,'nazwa','adres',809000900),(10,'Grzegorz','Warsaw',543111555),(21,'Klient','Radom',69696969);
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contract`
--

DROP TABLE IF EXISTS `contract`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contract` (
  `Contract_ID` int NOT NULL AUTO_INCREMENT,
  `Origin_address` varchar(50) NOT NULL,
  `Destination_address` varchar(50) NOT NULL,
  `Start_date` datetime NOT NULL,
  `Planned_end_date` datetime NOT NULL,
  `Real_end_date` datetime DEFAULT NULL,
  `Driver_ID` int NOT NULL,
  `Registration_number` varchar(7) NOT NULL,
  `Status` varchar(10) NOT NULL,
  `Client_ID` int NOT NULL,
  `Requested_seats` int NOT NULL,
  PRIMARY KEY (`Contract_ID`),
  KEY `Driver_ID` (`Driver_ID`),
  KEY `Registration_number` (`Registration_number`),
  KEY `Client_ID` (`Client_ID`),
  CONSTRAINT `contract_ibfk_1` FOREIGN KEY (`Driver_ID`) REFERENCES `driver` (`Driver_ID`),
  CONSTRAINT `contract_ibfk_2` FOREIGN KEY (`Registration_number`) REFERENCES `bus` (`Registration_number`),
  CONSTRAINT `contract_ibfk_3` FOREIGN KEY (`Client_ID`) REFERENCES `client` (`Client_ID`),
  CONSTRAINT `contract_chk_1` CHECK ((`Status` in (_utf8mb4'ACTIV',_utf8mb4'ON ROAD',_utf8mb4'DONE',_utf8mb4'CANCELED'))),
  CONSTRAINT `contract_chk_2` CHECK ((`Planned_end_date` >= `Start_date`)),
  CONSTRAINT `contract_chk_3` CHECK (((`Status` <> _utf8mb4'CANCELED') or (`Real_end_date` is null) or (`Real_end_date` <= `Start_date`)))
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contract`
--

LOCK TABLES `contract` WRITE;
/*!40000 ALTER TABLE `contract` DISABLE KEYS */;
INSERT INTO `contract` VALUES (42,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,2,'KR10001','ACTIV',1,0),(43,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0),(44,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0),(45,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0),(46,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0),(47,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0),(48,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0),(49,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0),(50,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0),(51,'Radom','Warszawa','2025-06-07 00:00:00','2025-06-08 00:00:00',NULL,1,'KR10001','ACTIV',1,0);
/*!40000 ALTER TABLE `contract` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `driver`
--

DROP TABLE IF EXISTS `driver`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `driver` (
  `Driver_ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Last_name` varchar(45) NOT NULL,
  `Driving_license_category` varchar(10) NOT NULL,
  `Driving_license_serial_number` int NOT NULL,
  PRIMARY KEY (`Driver_ID`),
  UNIQUE KEY `Driving_license_serial_number` (`Driving_license_serial_number`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `driver`
--

LOCK TABLES `driver` WRITE;
/*!40000 ALTER TABLE `driver` DISABLE KEYS */;
INSERT INTO `driver` VALUES (1,'Marek','Kowalski','C',12345),(2,'Magda','Gessler','D',123465),(7,'12132','11','11',111),(10,'Dominik','Duchnik','B',222),(13,'Dominik','Duchnik','B',333);
/*!40000 ALTER TABLE `driver` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-16 22:14:17
