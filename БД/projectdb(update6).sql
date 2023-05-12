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
-- Table structure for table `грамоты`
--

DROP TABLE IF EXISTS `грамоты`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `грамоты` (
  `Код_грамоты` int NOT NULL,
  `Наименование_грамоты` varchar(255) NOT NULL,
  `Путь_изображения` varchar(255) NOT NULL,
  `Преподаватель` int NOT NULL,
  PRIMARY KEY (`Код_грамоты`),
  KEY `Преподаватель_idx` (`Преподаватель`),
  CONSTRAINT `Преподаватель` FOREIGN KEY (`Преподаватель`) REFERENCES `преподаватели` (`Код_преподавателя`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `грамоты`
--

LOCK TABLES `грамоты` WRITE;
/*!40000 ALTER TABLE `грамоты` DISABLE KEYS */;
INSERT INTO `грамоты` VALUES (1,'За профессионализм в обучении ИТ-технологиям','cert1.png',2),(2,'Выдающийся преподаватель ИТ-дисциплин','cert2.png',2),(3,'За отличный вклад в развитие ИТ-образования','cert3.png',2),(4,'За высокий уровень преподавания ИТ-курсов','cert4.png',4),(5,'Сертификат','cert5.png',4);
/*!40000 ALTER TABLE `грамоты` ENABLE KEYS */;
UNLOCK TABLES;

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
INSERT INTO `материалы` VALUES (1,1,1,4,1,1,1,1,1,1,1,1),(2,1,2,2,1,1,1,0,1,1,0,1),(3,1,10,6,1,1,1,1,1,1,0,0),(4,1,7,4,1,1,1,1,1,1,1,1),(5,1,11,8,0,1,1,1,1,1,1,1),(6,1,13,3,0,1,1,1,1,1,1,1),(7,1,12,7,1,1,1,1,1,1,1,1),(8,1,8,7,1,1,1,1,1,1,1,1),(9,1,3,2,1,1,1,1,1,1,0,1),(10,1,4,2,1,1,1,0,1,1,0,1),(11,1,5,2,1,1,1,1,1,1,0,1),(12,1,6,5,1,1,1,1,1,1,0,0),(13,1,9,4,1,1,1,1,1,1,0,1);
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
  `Код_специальности` int NOT NULL,
  PRIMARY KEY (`Код_подразделения`),
  KEY `Код_пользователя` (`Код_пользователя`),
  KEY `Код_специальности_idx` (`Код_специальности`),
  CONSTRAINT `Код_специальности` FOREIGN KEY (`Код_специальности`) REFERENCES `специальности` (`Код_специальности`),
  CONSTRAINT `подразделение_ibfk_1` FOREIGN KEY (`Код_пользователя`) REFERENCES `пользователи` (`Код_пользователя`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `подразделение`
--

LOCK TABLES `подразделение` WRITE;
/*!40000 ALTER TABLE `подразделение` DISABLE KEYS */;
INSERT INTO `подразделение` VALUES (1,'Программирование в компьютерных системах',7,1,1);
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
  `Пароль` varchar(255) DEFAULT NULL,
  `Статус` varchar(85) NOT NULL,
  PRIMARY KEY (`Код_пользователя`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `пользователи`
--

LOCK TABLES `пользователи` WRITE;
/*!40000 ALTER TABLE `пользователи` DISABLE KEYS */;
INSERT INTO `пользователи` VALUES (1,'admin','21232F297A57A5A743894A0E4A801FC3','Администратор'),(2,'user','EE11CBB19052E40B07AAC0CA060C23EE','Методист'),(3,'bd','10E83D3070AE50C0AD75521A4C5883C1','Администратор БД'),(4,'Sadmin','EAD1A30FA7AA1DB6E84BD5C3B318B87B','Администратор'),(5,'admin2','69DC8B76502144B9D519827A4D9C2F56','Администратор'),(6,'bdadmin','21232F297A57A5A743894A0E4A801FC3','Администратор БД'),(7,'metodist','D940202C22EDAEBFD2908FB416F6BE21','Методист');
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
  `Индекс` varchar(90) NOT NULL,
  `Наименование_предмета` varchar(150) NOT NULL,
  `Код_специальности` varchar(90) NOT NULL,
  PRIMARY KEY (`Код_предмета`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `предметы`
--

LOCK TABLES `предметы` WRITE;
/*!40000 ALTER TABLE `предметы` DISABLE KEYS */;
INSERT INTO `предметы` VALUES (1,'ОП.04','Основы алгоритмизации и программирования','1'),(2,'ОП.08','Основы проектирования баз данных','1'),(3,'ОП.10','Численные методы','1'),(4,'ПМ.01','Разработка модулей программного обеспечения для компьютерных систем','1'),(5,'ПМ.02','Осуществление интеграции программных модулей','1'),(6,'ПМ.03','Ревьюирование программных продуктов','1'),(7,'ОП.09','Основы алгоритмизации и программирования','2'),(8,'ОП.11','Мультимедийные технологии','2'),(9,'ПМ.04','Сопровождение и обслуживание программного обеспечения компьютерных систем','1'),(10,'ПМ.05','Проектирование и разработка информационных систем','1'),(11,'ОУД.10','Информатика','4'),(12,'ОП.05','Информационные технологии','3'),(13,'ЭК.01','Основы компьютерной грамотности (ЭК)','4,1');
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
  `Путь_изображения` varchar(255) DEFAULT NULL,
  `Стаж_работы` varchar(55) DEFAULT NULL,
  `Специализация` varchar(155) DEFAULT NULL,
  `Телефон` varchar(255) DEFAULT NULL,
  `Почта` varchar(255) DEFAULT NULL,
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
INSERT INTO `преподаватели` VALUES (1,'Не работает',NULL,NULL,'',NULL,NULL,NULL,NULL),(2,'Николаева В. С.','1990-09-09',1,'person1.jpg','2 года','Разработка программного обеспечения','+7 (926) 123-45-67','m.kowalski5241@example.com'),(3,'Губанова К. М.','1999-01-01',1,'person2.png','4 года','Базы данных и управление данными','+7 (965) 789-01-23','s.petrov3698@example.com'),(4,'Крылова В. С.','1999-02-01',1,'person3.png','1 год','Сетевые технологии и безопасность','+7 (495) 234-56-78','a.smith2376@example.com'),(5,'Никитина С. К.','1999-02-01',1,'person4.png','5 лет','Веб-разработка и дизайн','+7 (929) 876-54-32','j.doe9463@example.com'),(6,'Антонова М. В.','1999-02-01',1,'person5.png','9 лет','Облачные технологии и виртуализация','+7 (916) 345-67-89','n.brown6829@example.com'),(7,'Смирнова М. М.','1999-02-01',1,'person6.png','10 лет','Веб-разработка и дизайн','+7 (926) 890-12-34','k.lee4512@example.com'),(8,'Николаева Э. Р.','1999-02-01',1,'person7.png','11 месяцев','Базы данных и управление данными','+7 (495) 567-89-01','d.jackson1834@example.com');
/*!40000 ALTER TABLE `преподаватели` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `специальности`
--

DROP TABLE IF EXISTS `специальности`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `специальности` (
  `Код_специальности` int NOT NULL,
  `Наименование` varchar(200) NOT NULL,
  PRIMARY KEY (`Код_специальности`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `специальности`
--

LOCK TABLES `специальности` WRITE;
/*!40000 ALTER TABLE `специальности` DISABLE KEYS */;
INSERT INTO `специальности` VALUES (1,'09.02.07 Информационные системы и программирование'),(2,'09.02.01 Компьютерные системы и комплексы'),(3,'Общепрофессиональные дисциплины специальностей'),(4,'Общеобразовательный цикл');
/*!40000 ALTER TABLE `специальности` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-05-09 22:45:57
