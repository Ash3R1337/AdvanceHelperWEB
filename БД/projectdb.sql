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
-- Table structure for table `материалы`
--

DROP TABLE IF EXISTS `материалы`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `материалы` (
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
  CONSTRAINT `материалы_ibfk_1` FOREIGN KEY (`Код_подразделения`) REFERENCES `подразделение` (`Код_подразделения`),
  CONSTRAINT `материалы_ibfk_2` FOREIGN KEY (`Код_предмета`) REFERENCES `предметы` (`Код_предмета`),
  CONSTRAINT `материалы_ibfk_3` FOREIGN KEY (`Код_преподавателя`) REFERENCES `преподаватели` (`Код_преподавателя`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `материалы`
--

LOCK TABLES `материалы` WRITE;
/*!40000 ALTER TABLE `материалы` DISABLE KEYS */;
INSERT INTO `материалы` VALUES (1,1,1,2,1,1,1,0,1,1,0,1),(2,1,1,3,1,1,1,1,1,1,1,1);
/*!40000 ALTER TABLE `материалы` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `подразделение`
--

DROP TABLE IF EXISTS `подразделение`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `подразделение` (
  `Код_подразделения` int NOT NULL,
  `Цикловая_комиссия` varchar(55) NOT NULL,
  `Количество_преподавателей` int NOT NULL,
  `Код_пользователя` int NOT NULL,
  PRIMARY KEY (`Код_подразделения`),
  KEY `Код_пользователя` (`Код_пользователя`),
  CONSTRAINT `подразделение_ibfk_1` FOREIGN KEY (`Код_пользователя`) REFERENCES `пользователи` (`Код_пользователя`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `подразделение`
--

LOCK TABLES `подразделение` WRITE;
/*!40000 ALTER TABLE `подразделение` DISABLE KEYS */;
INSERT INTO `подразделение` VALUES (1,'Программирование компьютерных систем',2,1);
/*!40000 ALTER TABLE `подразделение` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `пользователи`
--

DROP TABLE IF EXISTS `пользователи`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `пользователи` (
  `Код_пользователя` int NOT NULL,
  `Логин` varchar(15) NOT NULL,
  `Пароль` varchar(15) NOT NULL,
  `Статус` varchar(85) NOT NULL,
  PRIMARY KEY (`Код_пользователя`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `пользователи`
--

LOCK TABLES `пользователи` WRITE;
/*!40000 ALTER TABLE `пользователи` DISABLE KEYS */;
INSERT INTO `пользователи` VALUES (1,'admin','password1','Начальник мет. отдела'),(2,'user','pssIdd','Методист'),(3,'bd','bdadminq3','Администратор БД'),(4,'Sadmin','lkheHELf21sda','Администратор'),(5,'admin','admin','Администратор');
/*!40000 ALTER TABLE `пользователи` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `предметы`
--

DROP TABLE IF EXISTS `предметы`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `предметы` (
  `Код_предмета` int NOT NULL,
  `Наименование_предмета` varchar(90) NOT NULL,
  PRIMARY KEY (`Код_предмета`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `предметы`
--

LOCK TABLES `предметы` WRITE;
/*!40000 ALTER TABLE `предметы` DISABLE KEYS */;
INSERT INTO `предметы` VALUES (1,'ПМ03'),(2,'ПМ06'),(3,'Операционные системы'),(5,'ПМ01'),(6,'ПМ04');
/*!40000 ALTER TABLE `предметы` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `преподаватели`
--

DROP TABLE IF EXISTS `преподаватели`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `преподаватели` (
  `Код_преподавателя` int NOT NULL,
  `ФИО` varchar(90) NOT NULL,
  `Дата_рождения` date DEFAULT NULL,
  `Код_подразделения` int DEFAULT NULL,
  PRIMARY KEY (`Код_преподавателя`),
  KEY `Код_подразделения` (`Код_подразделения`),
  CONSTRAINT `преподаватели_ibfk_1` FOREIGN KEY (`Код_подразделения`) REFERENCES `подразделение` (`Код_подразделения`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `преподаватели`
--

LOCK TABLES `преподаватели` WRITE;
/*!40000 ALTER TABLE `преподаватели` DISABLE KEYS */;
INSERT INTO `преподаватели` VALUES (1,'Не работает',NULL,NULL),(2,'Гунько Ирина Александровна','1990-09-09',1),(3,'Деркун Александр Викторович','1999-01-01',1);
/*!40000 ALTER TABLE `преподаватели` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-02-05 23:21:42
