-- MySQL dump 10.13  Distrib 8.0.31, for Win64 (x86_64)
--
-- Host: localhost    Database: projectdb
-- ------------------------------------------------------
-- Server version	8.0.31

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
-- Table structure for table `materials`
--

DROP TABLE IF EXISTS `materials`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `materials` (
  `Код_Материала` int NOT NULL,
  `Код_подразделения` int NOT NULL,
  `Код_предмета` int NOT NULL,
  `Код_преподавателя` int NOT NULL,
  `Титул_РП` tinyint(1) NOT NULL,
  `РП` tinyint(1) NOT NULL,
  `Титул_ФОС` tinyint(1) NOT NULL,
  `ФОС` tinyint(1) NOT NULL,
  `ВнутрРец` tinyint(1) NOT NULL,
  `ЭкспЗакл` tinyint(1) NOT NULL,
  `ВСРС` tinyint(1) NOT NULL,
  `МУПР` tinyint(1) NOT NULL,
  PRIMARY KEY (`Код_Материала`),
  KEY `Код_подразделения` (`Код_подразделения`),
  KEY `Код_предмета` (`Код_предмета`),
  KEY `Код_преподавателя` (`Код_преподавателя`),
  CONSTRAINT `materials_ibfk_1` FOREIGN KEY (`Код_подразделения`) REFERENCES `subdivision` (`Код_подразделения`),
  CONSTRAINT `materials_ibfk_2` FOREIGN KEY (`Код_предмета`) REFERENCES `subjects` (`Код_предмета`),
  CONSTRAINT `materials_ibfk_3` FOREIGN KEY (`Код_преподавателя`) REFERENCES `teachers` (`Код_преподавателя`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `materials`
--

LOCK TABLES `materials` WRITE;
/*!40000 ALTER TABLE `materials` DISABLE KEYS */;
INSERT INTO `materials` VALUES (1,1,1,2,1,1,1,0,1,1,0,1),(2,1,1,3,1,1,1,1,1,1,1,1);
/*!40000 ALTER TABLE `materials` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-02-01 23:58:27
