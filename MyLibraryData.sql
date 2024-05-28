-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: coursework
-- ------------------------------------------------------
-- Server version	8.3.0

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
-- Table structure for table `author`
--

DROP TABLE IF EXISTS `author`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `author` (
  `id` int NOT NULL AUTO_INCREMENT,
  `pen_name` varchar(45) DEFAULT NULL,
  `first_name` varchar(45) DEFAULT NULL,
  `last_name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `author`
--

LOCK TABLES `author` WRITE;
/*!40000 ALTER TABLE `author` DISABLE KEYS */;
INSERT INTO `author` VALUES (1,'Айн Ренд','Аліса','Розенбаум'),(2,NULL,'Софія','Андрухович'),(3,NULL,'Фредрік','Бакман'),(4,NULL,'Степан','Бандера'),(5,NULL,'Дмитро','Капранов'),(6,NULL,'Віталій','Капранов'),(7,NULL,'Володимер','В\'ятронович'),(8,'Роберт Ґалбрейт','Джоан','Роулінг'),(9,NULL,'Ярослав','Грицак'),(10,NULL,'Колін','Гувер'),(11,NULL,'Роальд','Дал'),(12,'Люко Дашвар','Ірина','Чернова'),(13,NULL,'Сергій','Жадан'),(14,'Кідрук Макс','Максим','Кідрук'),(15,NULL,'Стівен','Кінг'),(16,NULL,'Агата','Крісті'),(17,'Марк Лівін','Валерій','Катерушин'),(18,NULL,'Астрід','Ліндґрен'),(19,NULL,'Андрій','Любка'),(20,NULL,'Всеволод','Нестайко'),(21,NULL,'Нікіта','Тітов'),(22,'Ілля Полудьонний','Ілля','Полудьонний'),(23,'Ілларіон Павлюк','Ілларіон','Павлюк'),(24,NULL,'Ліна','Костенко'),(25,NULL,'Андрій','Сем’янків');
/*!40000 ALTER TABLE `author` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `book`
--

DROP TABLE IF EXISTS `book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `book` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(100) NOT NULL,
  `amount_of_all` int NOT NULL,
  `amount_of_stoke` int NOT NULL,
  `publishing_id` int DEFAULT NULL,
  `year_of_publication` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_publising_idx` (`publishing_id`),
  CONSTRAINT `fk_publising` FOREIGN KEY (`publishing_id`) REFERENCES `publishing_house` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `book`
--

LOCK TABLES `book` WRITE;
/*!40000 ALTER TABLE `book` DISABLE KEYS */;
INSERT INTO `book` VALUES (1,'Наша столітня. Короткі нариси про довгу війну',10,9,20,2024),(2,'Примха мерця',8,7,2,2023),(4,'Сторітелінг для очей, вух і серця',5,5,33,2020),(5,'Простими словами. Як розібратися у своїх емоціях',15,15,33,2021),(6,'Зелена,19',12,11,51,2020),(7,'Тривожні люди',4,3,51,2021),(8,'Село не люди',5,5,52,2019),(9,'Село не люди',10,10,29,2021),(10,'Я бачу, вас цікавить пітьма',20,20,13,2020),(11,'Триста поезій',20,20,1,2019),(12,'Танці з кістками',16,16,18,2022);
/*!40000 ALTER TABLE `book` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `book_author`
--

DROP TABLE IF EXISTS `book_author`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `book_author` (
  `book_id` int NOT NULL,
  `author_id` int NOT NULL,
  PRIMARY KEY (`book_id`,`author_id`),
  KEY `fk_book_has_author_author1_idx` (`author_id`),
  KEY `fk_book_has_author_book_idx` (`book_id`),
  CONSTRAINT `fk_book_has_author_author1` FOREIGN KEY (`author_id`) REFERENCES `author` (`id`),
  CONSTRAINT `fk_book_has_author_book` FOREIGN KEY (`book_id`) REFERENCES `book` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `book_author`
--

LOCK TABLES `book_author` WRITE;
/*!40000 ALTER TABLE `book_author` DISABLE KEYS */;
INSERT INTO `book_author` VALUES (7,3),(1,7),(8,12),(9,12),(2,16),(4,17),(5,17),(6,17),(1,21),(5,22),(10,23),(11,24),(12,25);
/*!40000 ALTER TABLE `book_author` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `book_genre`
--

DROP TABLE IF EXISTS `book_genre`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `book_genre` (
  `book_id` int NOT NULL,
  `genre_id` int NOT NULL,
  PRIMARY KEY (`book_id`,`genre_id`),
  KEY `fk_book_has_genre_genre1_idx` (`genre_id`),
  KEY `fk_book_has_genre_book1_idx` (`book_id`),
  CONSTRAINT `fk_book_has_genre_book1` FOREIGN KEY (`book_id`) REFERENCES `book` (`id`),
  CONSTRAINT `fk_book_has_genre_genre1` FOREIGN KEY (`genre_id`) REFERENCES `genre` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `book_genre`
--

LOCK TABLES `book_genre` WRITE;
/*!40000 ALTER TABLE `book_genre` DISABLE KEYS */;
INSERT INTO `book_genre` VALUES (10,1),(2,3),(10,3),(7,4),(10,4),(4,5),(1,8),(10,9),(11,10),(4,14),(5,14),(7,14),(4,15),(5,15),(6,17),(10,19),(12,19),(8,22),(9,22),(8,23),(9,23);
/*!40000 ALTER TABLE `book_genre` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `id` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(45) NOT NULL,
  `last_name` varchar(45) NOT NULL,
  `phone` varchar(16) NOT NULL,
  `password` varchar(16) NOT NULL,
  `sub_end_date` date NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,'Джон','Сміт','+1234567890','1','2025-05-27'),(2,'Емма','Джонсон','+1234567891','102','2024-06-29'),(3,'Майкл','Девіс','+1234567892','103','2024-05-20'),(4,'Ханна','Браун','+1234567893','104','2024-05-23'),(5,'Софія','Уільямс','+1234567894','105','2024-05-24'),(6,'Девід','Міллер','+1234567895','106','2024-05-25'),(7,'Олівер','Тейлор','+1234567896','107','2024-05-26'),(8,'Евелін','Вілсон','+1234567897','108','2024-05-27'),(9,'Ізабель','Мур','+1234567898','109','2024-05-28'),(10,'Джеймс','Харріс','+1234567899','110','2024-05-30'),(11,'Егор','Петров','+9876567893','111','2024-06-01'),(12,'Михайло','Ферліковський','+380567483930','123','2024-06-25');
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `genre`
--

DROP TABLE IF EXISTS `genre`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `genre` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `genre`
--

LOCK TABLES `genre` WRITE;
/*!40000 ALTER TABLE `genre` DISABLE KEYS */;
INSERT INTO `genre` VALUES (1,'Фантастика'),(2,'Фентезі'),(3,'Детектив'),(4,'Роман'),(5,'Наукова література'),(6,'Містика'),(7,'Біографія'),(8,'Історичний роман'),(9,'Пригоди'),(10,'Поезія'),(11,'Драматургія'),(12,'Езотерика'),(13,'Мемуари'),(14,'Психологія'),(15,'Саморозвиток'),(16,'Навчальна література'),(17,'Дитяча література'),(18,'Сатира'),(19,'Трилер'),(20,'Горор'),(21,'Автобіографія'),(22,'Фікшин'),(23,'Містерія');
/*!40000 ALTER TABLE `genre` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `issue_of_book`
--

DROP TABLE IF EXISTS `issue_of_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `issue_of_book` (
  `id` int NOT NULL AUTO_INCREMENT,
  `book_id` int NOT NULL,
  `client_id` int NOT NULL,
  `librarian_id` int NOT NULL,
  `date` date NOT NULL,
  `return_book` enum('true','false') NOT NULL,
  PRIMARY KEY (`id`,`book_id`,`client_id`,`librarian_id`),
  KEY `fk_issue_of_book_client1_idx` (`client_id`),
  KEY `fk_issue_of_book_book1_idx` (`book_id`),
  KEY `fk_issue_of_book_librarian1_idx` (`librarian_id`),
  CONSTRAINT `fk_issue_of_book_book1` FOREIGN KEY (`book_id`) REFERENCES `book` (`id`),
  CONSTRAINT `fk_issue_of_book_client1` FOREIGN KEY (`client_id`) REFERENCES `client` (`id`),
  CONSTRAINT `fk_issue_of_book_librarian1` FOREIGN KEY (`librarian_id`) REFERENCES `librarian` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `issue_of_book`
--

LOCK TABLES `issue_of_book` WRITE;
/*!40000 ALTER TABLE `issue_of_book` DISABLE KEYS */;
INSERT INTO `issue_of_book` VALUES (1,1,1,1,'2024-04-27','true'),(2,9,1,1,'2024-05-27','true'),(3,12,1,1,'2024-04-22','true'),(4,4,1,1,'2024-05-28','true'),(5,11,1,1,'2024-05-28','true'),(6,1,1,1,'2024-05-28','true'),(7,2,1,1,'2024-05-28','true'),(8,4,1,1,'2024-05-28','true'),(9,5,1,1,'2024-05-29','true'),(10,7,1,1,'2024-05-29','false'),(11,6,1,1,'2024-06-29','false');
/*!40000 ALTER TABLE `issue_of_book` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `librarian`
--

DROP TABLE IF EXISTS `librarian`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `librarian` (
  `id` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(45) NOT NULL,
  `last_name` varchar(45) NOT NULL,
  `phone` varchar(16) NOT NULL,
  `password` varchar(16) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `librarian`
--

LOCK TABLES `librarian` WRITE;
/*!40000 ALTER TABLE `librarian` DISABLE KEYS */;
INSERT INTO `librarian` VALUES (1,'Ярослав','Солодуха','0987654321','666');
/*!40000 ALTER TABLE `librarian` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `publishing_house`
--

DROP TABLE IF EXISTS `publishing_house`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `publishing_house` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `publishing_house`
--

LOCK TABLES `publishing_house` WRITE;
/*!40000 ALTER TABLE `publishing_house` DISABLE KEYS */;
INSERT INTO `publishing_house` VALUES (1,'А-ба-ба-га-ла-ма-га'),(2,'Аверс'),(3,'Апріорі'),(4,'АССА'),(5,'Астролябія'),(6,'Білка'),(7,'Брайт Букс'),(8,'Веселка'),(9,'Видавництво \"Видавництво\"'),(10,'Видавництво XXI'),(11,'Видавництво Жупанського'),(12,'Видавництво імені Олени Теліги'),(13,'Видавництво Старого Лева'),(14,'Видавнича група \"Основа\"'),(15,'Видавничий дім \"Києво-Могилянська академія\"'),(16,'Видавничий дім \"Школа\"'),(17,'Віват'),(18,'Віхола'),(19,'Грані-Т'),(20,'Дитяча редакція видавництва \"Наш Формат\"'),(21,'Дніпро'),(22,'Дух і Літера'),(23,'Жорж'),(24,'Зелений Пес'),(25,'Карпатська вежа'),(26,'КМ-Букс'),(27,'Комора'),(28,'Комубук'),(29,'КСД'),(30,'Лабораторія'),(31,'Моноліт-Bizz'),(32,'Навчальна книга - Богдан'),(33,'Наш Формат'),(34,'Основи'),(35,'Пелікан'),(36,'Піраміда'),(37,'Портал'),(38,'Пропала грамота'),(39,'Ранок'),(40,'РМ'),(41,'Свічадо'),(42,'Смолоскип'),(43,'Теза'),(44,'Темпора'),(45,'Фабула'),(46,'Фоліо'),(47,'ФОП Стебеляк'),(48,'Човен'),(49,'Чорні вівці'),(50,'Ярославів Вал'),(51,'#книголав'),(52,'Книжковий клуб \"Клуб Сімейного Дозвілля\"');
/*!40000 ALTER TABLE `publishing_house` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'coursework'
--

--
-- Dumping routines for database 'coursework'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-05-29  2:05:58
