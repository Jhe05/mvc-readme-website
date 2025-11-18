-- MariaDB dump 10.19  Distrib 10.4.32-MariaDB, for Win64 (AMD64)
--
-- Host: 127.0.0.1    Database: mvcreadme_db
-- ------------------------------------------------------
-- Server version	10.4.32-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` text DEFAULT NULL,
  `ProductVersion` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20250929121639_InitialMigration','9.0.0'),('20250929145454_AddAvatarPathToUser','9.0.0'),('20251026161345_AddUserNameAndEmailFields','9.0.0'),('20251028161216_AddBookReferenceToBookRead','9.0.0');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__efmigrationslock`
--

DROP TABLE IF EXISTS `__efmigrationslock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__efmigrationslock` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Timestamp` text DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationslock`
--

LOCK TABLES `__efmigrationslock` WRITE;
/*!40000 ALTER TABLE `__efmigrationslock` DISABLE KEYS */;
/*!40000 ALTER TABLE `__efmigrationslock` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `admin_notifications`
--

DROP TABLE IF EXISTS `admin_notifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `admin_notifications` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `message` text NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `is_read` tinyint(1) DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_notifications`
--

LOCK TABLES `admin_notifications` WRITE;
/*!40000 ALTER TABLE `admin_notifications` DISABLE KEYS */;
INSERT INTO `admin_notifications` VALUES (1,'User nicko_712 rated Python Crash Course with 5 stars.','2025-11-05 15:25:39',1),(2,'User Venus rated The Midnight Library with 5 stars.','2025-11-05 15:27:48',1),(3,'User Venus rated Data Science from Scratch with 5 stars.','2025-11-05 15:32:27',1),(4,'User Venus rated Atomic Habits with 5 stars.','2025-11-06 15:14:43',1),(5,'User Venus rated Ego is the Enemy with 5 stars.','2025-11-06 16:00:09',1),(6,'User Venus rated A Gentle Reminder with 5 stars.','2025-11-06 16:05:15',1),(7,'User Venus rated Zero to One with 5 stars.','2025-11-06 16:09:01',1),(8,'User Venus rated Ikigai with 5 stars.','2025-11-06 16:10:43',1),(9,'User Venus rated Algorithms to Live By with 5 stars.','2025-11-06 16:14:06',1),(10,'User Venus rated Sherlock Holmes with 5 stars.','2025-11-06 16:16:20',1),(11,'User Venus rated The Alchemist with 5 stars.','2025-11-06 16:21:02',1),(12,'User Venus rated 48 Laws of Power with 5 stars.','2025-11-06 16:23:14',1),(13,'User Venus rated Man\\\'s Search for Meaning with 5 stars.','2025-11-08 14:39:08',1),(14,'User Venus rated Introduction to Algorithms with 5 stars.','2025-11-08 15:06:30',1),(15,'User Venus rated Grokking Deep Learning with 5 stars.','2025-11-08 15:06:36',1),(16,'User jheris456 rated Ego is the Enemy with 5 stars.','2025-11-08 15:07:20',1),(17,'User jheris456 rated Python Crash Course with 5 stars.','2025-11-08 15:07:24',1),(18,'User Venus rated Sapiens with 5 stars.','2025-11-09 14:33:28',1),(19,'User Venus rated Think and Grow Rich with 5 stars.','2025-11-09 14:55:47',1),(20,'User Venus rated The Hobbit with 5 stars.','2025-11-09 18:27:02',1),(21,'User Venus commented on The Hobbit: Good Book!!','2025-11-09 17:27:17',1),(22,'User Venus rated The Psychology of Money with 4 stars.','2025-11-09 18:32:08',1),(23,'User Venus rated The Psychology of Money with 5 stars.','2025-11-09 18:32:09',1),(24,'[CommentId:8] User Venus commented on The Psychology of Money: fbgfdfdg','2025-11-09 17:32:13',1),(25,'[CommentId:9] User Venus commented on The Midnight Library: fdsfdsfdsf','2025-11-09 17:36:34',1),(26,'[CommentId:10] User Venus commented on The Midnight Library: wdad','2025-11-09 17:40:12',1),(27,'User Venus rated Python Crash Course with 5 stars.','2025-11-09 18:44:27',1),(28,'User Venus commented on The Midnight Library: awd','2025-11-09 17:58:02',1),(29,'User Venus commented on Data Science from Scratch: grdgdg','2025-11-09 18:06:29',1),(30,'User Venus commented on Atomic Habits: dada','2025-11-10 13:42:26',1),(31,'User jheris456 rated Algorithms to Live By with 5 stars.','2025-11-10 14:44:04',1),(32,'User Venus commented on The Midnight Library: csaasd','2025-11-11 15:49:00',1),(33,'User Venus commented on Data Science from Scratch: helloo','2025-11-13 09:53:51',1),(34,'User jheris456 rated The Little Prince with 5 stars.','2025-11-13 15:45:06',1),(35,'User jheris456 commented on The Little Prince: ewfwe','2025-11-13 14:45:10',1),(36,'User Venus commented on Man\'s Search for Meaning: dwad','2025-11-13 15:09:25',1),(37,'User Venus rated The History Book with 5 stars.','2025-11-13 16:33:12',1),(38,'User Venus commented on The History Book: gfg','2025-11-13 15:33:18',1),(39,'User Venus commented on The History Book: fsdsf','2025-11-13 15:38:35',1),(40,'User Venus commented on The Mountain is You: thrthrt','2025-11-13 15:51:16',1),(41,'User Venus commented on The Midnight Library: hello','2025-11-13 16:02:29',1),(42,'User Venus commented on Ikigai: fsefsf','2025-11-13 16:03:14',1),(43,'User Venus commented on Atomic Habits: dsfdf','2025-11-13 17:24:58',1),(44,'User Venus commented on Atomic Habits: dsfdf','2025-11-13 17:24:58',1),(45,'User Venus commented on Atomic Habits: fdfds','2025-11-13 17:25:05',1),(46,'User Venus commented on 48 Laws of Power: hello','2025-11-13 17:26:10',1),(47,'User jheris456 commented on The Little Prince: jheris','2025-11-13 17:27:11',1),(48,'User jheris456 rated Man\\\'s Search for Meaning with 5 stars.','2025-11-13 18:32:26',1),(49,'User Venus rated Philosophy 101 with 5 stars.','2025-11-14 00:10:30',1),(50,'User jheris456 rated Philosophy 101 with 5 stars.','2025-11-14 00:11:26',1),(51,'User Venus rated The Compound Effect with 5 stars.','2025-11-14 02:23:00',1),(52,'User Venus commented on The Compound Effect: gdgd','2025-11-14 01:23:08',1),(53,'User Venus rated Memory Man with 5 stars.','2025-11-14 04:32:44',1),(54,'User Venus commented on Memory Man: hello','2025-11-14 03:32:51',1);
/*!40000 ALTER TABLE `admin_notifications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `book_ratings`
--

DROP TABLE IF EXISTS `book_ratings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `book_ratings` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `book_id` bigint(20) NOT NULL,
  `user_id` bigint(20) NOT NULL,
  `rating` tinyint(4) NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `ux_book_user` (`book_id`,`user_id`),
  KEY `idx_book_id` (`book_id`),
  KEY `idx_user_id` (`user_id`),
  CONSTRAINT `fk_bookratings_book` FOREIGN KEY (`book_id`) REFERENCES `books` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_bookratings_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `book_ratings`
--

LOCK TABLES `book_ratings` WRITE;
/*!40000 ALTER TABLE `book_ratings` DISABLE KEYS */;
INSERT INTO `book_ratings` VALUES (1,1,2,5,'2025-11-05 15:25:39'),(2,3,17,5,'2025-11-05 15:27:48'),(3,4,17,5,'2025-11-05 15:32:27'),(4,2,17,5,'2025-11-06 15:14:43'),(5,7,17,5,'2025-11-06 16:00:09'),(6,11,17,5,'2025-11-06 16:05:15'),(7,17,17,5,'2025-11-06 16:09:01'),(8,5,17,5,'2025-11-06 16:10:43'),(9,8,17,5,'2025-11-06 16:14:06'),(10,27,17,5,'2025-11-06 16:16:20'),(11,14,17,5,'2025-11-06 16:21:02'),(12,26,17,5,'2025-11-06 16:23:14'),(13,28,17,5,'2025-11-08 14:39:08'),(14,6,17,5,'2025-11-08 15:06:30'),(15,13,17,5,'2025-11-08 15:06:36'),(16,7,3,5,'2025-11-08 15:07:20'),(17,1,3,5,'2025-11-08 15:07:24'),(18,15,17,5,'2025-11-09 14:33:28'),(19,18,17,5,'2025-11-09 14:55:47'),(20,22,17,5,'2025-11-09 18:27:02'),(21,12,17,5,'2025-11-09 18:32:09'),(22,1,17,5,'2025-11-09 18:44:27'),(23,8,3,5,'2025-11-10 14:44:04'),(24,29,3,5,'2025-11-13 15:45:06'),(25,30,17,5,'2025-11-13 16:33:12'),(26,28,3,5,'2025-11-13 18:32:26'),(27,10,17,5,'2025-11-14 00:10:30'),(28,10,3,5,'2025-11-14 00:11:26'),(29,23,17,5,'2025-11-14 02:23:00'),(30,21,17,5,'2025-11-14 04:32:44');
/*!40000 ALTER TABLE `book_ratings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bookreads`
--

DROP TABLE IF EXISTS `bookreads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bookreads` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `BookId` bigint(20) DEFAULT NULL,
  `ReadCount` bigint(20) DEFAULT NULL,
  `ReadDate` text DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=264 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bookreads`
--

LOCK TABLES `bookreads` WRITE;
/*!40000 ALTER TABLE `bookreads` DISABLE KEYS */;
INSERT INTO `bookreads` VALUES (1,1,43,'2025-10-29 00:00:00'),(2,1,1,'2025-10-28 00:00:00'),(3,1,1,'2025-10-27 00:00:00'),(4,1,1,'2025-10-26 00:00:00'),(5,1,0,'2025-10-25 00:00:00'),(6,1,1,'2025-10-24 00:00:00'),(7,1,0,'2025-10-23 00:00:00'),(8,2,1,'2025-10-29 00:00:00'),(9,2,1,'2025-10-28 00:00:00'),(10,2,1,'2025-10-27 00:00:00'),(11,2,1,'2025-10-26 00:00:00'),(12,2,0,'2025-10-25 00:00:00'),(13,2,1,'2025-10-24 00:00:00'),(14,2,0,'2025-10-23 00:00:00'),(15,3,1,'2025-10-29 00:00:00'),(16,3,1,'2025-10-28 00:00:00'),(17,3,1,'2025-10-27 00:00:00'),(18,3,1,'2025-10-26 00:00:00'),(19,3,0,'2025-10-25 00:00:00'),(20,3,1,'2025-10-24 00:00:00'),(21,3,0,'2025-10-23 00:00:00'),(22,4,1,'2025-10-29 00:00:00'),(23,4,1,'2025-10-28 00:00:00'),(24,4,1,'2025-10-27 00:00:00'),(25,4,1,'2025-10-26 00:00:00'),(26,4,0,'2025-10-25 00:00:00'),(27,4,1,'2025-10-24 00:00:00'),(28,4,0,'2025-10-23 00:00:00'),(29,5,1,'2025-10-29 00:00:00'),(30,5,1,'2025-10-28 00:00:00'),(31,5,1,'2025-10-27 00:00:00'),(32,5,1,'2025-10-26 00:00:00'),(33,5,0,'2025-10-25 00:00:00'),(34,5,1,'2025-10-24 00:00:00'),(35,5,0,'2025-10-23 00:00:00'),(36,6,1,'2025-10-29 00:00:00'),(37,6,1,'2025-10-28 00:00:00'),(38,6,1,'2025-10-27 00:00:00'),(39,6,1,'2025-10-26 00:00:00'),(40,6,0,'2025-10-25 00:00:00'),(41,6,1,'2025-10-24 00:00:00'),(42,6,0,'2025-10-23 00:00:00'),(43,7,1,'2025-10-29 00:00:00'),(44,7,1,'2025-10-28 00:00:00'),(45,7,1,'2025-10-27 00:00:00'),(46,7,1,'2025-10-26 00:00:00'),(47,7,0,'2025-10-25 00:00:00'),(48,7,1,'2025-10-24 00:00:00'),(49,7,0,'2025-10-23 00:00:00'),(50,8,1,'2025-10-29 00:00:00'),(51,8,1,'2025-10-28 00:00:00'),(52,8,1,'2025-10-27 00:00:00'),(53,8,1,'2025-10-26 00:00:00'),(54,8,0,'2025-10-25 00:00:00'),(55,8,1,'2025-10-24 00:00:00'),(56,8,0,'2025-10-23 00:00:00'),(57,9,1,'2025-10-29 00:00:00'),(58,9,1,'2025-10-28 00:00:00'),(59,9,1,'2025-10-27 00:00:00'),(60,9,1,'2025-10-26 00:00:00'),(61,9,0,'2025-10-25 00:00:00'),(62,9,1,'2025-10-24 00:00:00'),(63,9,0,'2025-10-23 00:00:00'),(64,10,1,'2025-10-29 00:00:00'),(65,10,1,'2025-10-28 00:00:00'),(66,10,1,'2025-10-27 00:00:00'),(67,10,1,'2025-10-26 00:00:00'),(68,10,0,'2025-10-25 00:00:00'),(69,10,1,'2025-10-24 00:00:00'),(70,10,0,'2025-10-23 00:00:00'),(71,11,1,'2025-10-29 00:00:00'),(72,11,1,'2025-10-28 00:00:00'),(73,11,1,'2025-10-27 00:00:00'),(74,11,1,'2025-10-26 00:00:00'),(75,11,0,'2025-10-25 00:00:00'),(76,11,1,'2025-10-24 00:00:00'),(77,11,0,'2025-10-23 00:00:00'),(78,12,1,'2025-10-29 00:00:00'),(79,12,1,'2025-10-28 00:00:00'),(80,12,1,'2025-10-27 00:00:00'),(81,12,1,'2025-10-26 00:00:00'),(82,12,0,'2025-10-25 00:00:00'),(83,12,1,'2025-10-24 00:00:00'),(84,12,0,'2025-10-23 00:00:00'),(85,13,1,'2025-10-29 00:00:00'),(86,13,1,'2025-10-28 00:00:00'),(87,13,1,'2025-10-27 00:00:00'),(88,13,1,'2025-10-26 00:00:00'),(89,13,0,'2025-10-25 00:00:00'),(90,13,1,'2025-10-24 00:00:00'),(91,13,0,'2025-10-23 00:00:00'),(92,14,1,'2025-10-29 00:00:00'),(93,14,1,'2025-10-28 00:00:00'),(94,14,1,'2025-10-27 00:00:00'),(95,14,1,'2025-10-26 00:00:00'),(96,14,0,'2025-10-25 00:00:00'),(97,14,1,'2025-10-24 00:00:00'),(98,14,0,'2025-10-23 00:00:00'),(99,15,1,'2025-10-29 00:00:00'),(100,15,1,'2025-10-28 00:00:00'),(101,15,1,'2025-10-27 00:00:00'),(102,15,1,'2025-10-26 00:00:00'),(103,15,0,'2025-10-25 00:00:00'),(104,15,1,'2025-10-24 00:00:00'),(105,15,0,'2025-10-23 00:00:00'),(106,16,1,'2025-10-29 00:00:00'),(107,16,1,'2025-10-28 00:00:00'),(108,16,1,'2025-10-27 00:00:00'),(109,16,1,'2025-10-26 00:00:00'),(110,16,0,'2025-10-25 00:00:00'),(111,16,1,'2025-10-24 00:00:00'),(112,16,0,'2025-10-23 00:00:00'),(113,17,1,'2025-10-29 00:00:00'),(114,17,1,'2025-10-28 00:00:00'),(115,17,1,'2025-10-27 00:00:00'),(116,17,1,'2025-10-26 00:00:00'),(117,17,0,'2025-10-25 00:00:00'),(118,17,1,'2025-10-24 00:00:00'),(119,17,0,'2025-10-23 00:00:00'),(120,18,1,'2025-10-29 00:00:00'),(121,18,1,'2025-10-28 00:00:00'),(122,18,1,'2025-10-27 00:00:00'),(123,18,1,'2025-10-26 00:00:00'),(124,18,0,'2025-10-25 00:00:00'),(125,18,1,'2025-10-24 00:00:00'),(126,18,0,'2025-10-23 00:00:00'),(127,19,1,'2025-10-29 00:00:00'),(128,19,1,'2025-10-28 00:00:00'),(129,19,1,'2025-10-27 00:00:00'),(130,19,1,'2025-10-26 00:00:00'),(131,19,0,'2025-10-25 00:00:00'),(132,19,1,'2025-10-24 00:00:00'),(133,19,0,'2025-10-23 00:00:00'),(134,20,1,'2025-10-29 00:00:00'),(135,20,1,'2025-10-28 00:00:00'),(136,20,1,'2025-10-27 00:00:00'),(137,20,1,'2025-10-26 00:00:00'),(138,20,0,'2025-10-25 00:00:00'),(139,20,1,'2025-10-24 00:00:00'),(140,20,0,'2025-10-23 00:00:00'),(141,21,1,'2025-10-29 00:00:00'),(142,21,1,'2025-10-28 00:00:00'),(143,21,1,'2025-10-27 00:00:00'),(144,21,1,'2025-10-26 00:00:00'),(145,21,0,'2025-10-25 00:00:00'),(146,21,1,'2025-10-24 00:00:00'),(147,21,0,'2025-10-23 00:00:00'),(148,22,1,'2025-10-29 00:00:00'),(149,22,1,'2025-10-28 00:00:00'),(150,22,1,'2025-10-27 00:00:00'),(151,22,1,'2025-10-26 00:00:00'),(152,22,0,'2025-10-25 00:00:00'),(153,22,1,'2025-10-24 00:00:00'),(154,22,0,'2025-10-23 00:00:00'),(155,23,1,'2025-10-29 00:00:00'),(156,23,1,'2025-10-28 00:00:00'),(157,23,1,'2025-10-27 00:00:00'),(158,23,1,'2025-10-26 00:00:00'),(159,23,0,'2025-10-25 00:00:00'),(160,23,1,'2025-10-24 00:00:00'),(161,23,0,'2025-10-23 00:00:00'),(162,24,1,'2025-10-29 00:00:00'),(163,24,1,'2025-10-28 00:00:00'),(164,24,1,'2025-10-27 00:00:00'),(165,24,1,'2025-10-26 00:00:00'),(166,24,0,'2025-10-25 00:00:00'),(167,24,1,'2025-10-24 00:00:00'),(168,24,0,'2025-10-23 00:00:00'),(169,25,1,'2025-10-29 00:00:00'),(170,25,1,'2025-10-28 00:00:00'),(171,25,1,'2025-10-27 00:00:00'),(172,25,1,'2025-10-26 00:00:00'),(173,25,0,'2025-10-25 00:00:00'),(174,25,1,'2025-10-24 00:00:00'),(175,25,0,'2025-10-23 00:00:00'),(176,26,1,'2025-10-29 00:00:00'),(177,26,1,'2025-10-28 00:00:00'),(178,26,1,'2025-10-27 00:00:00'),(179,26,1,'2025-10-26 00:00:00'),(180,26,0,'2025-10-25 00:00:00'),(181,26,1,'2025-10-24 00:00:00'),(182,26,0,'2025-10-23 00:00:00'),(183,27,1,'2025-10-29 00:00:00'),(184,27,1,'2025-10-28 00:00:00'),(185,27,1,'2025-10-27 00:00:00'),(186,27,1,'2025-10-26 00:00:00'),(187,27,0,'2025-10-25 00:00:00'),(188,27,1,'2025-10-24 00:00:00'),(189,27,0,'2025-10-23 00:00:00'),(190,28,1,'2025-10-29 00:00:00'),(191,28,1,'2025-10-28 00:00:00'),(192,28,1,'2025-10-27 00:00:00'),(193,28,1,'2025-10-26 00:00:00'),(194,28,0,'2025-10-25 00:00:00'),(195,28,1,'2025-10-24 00:00:00'),(196,28,0,'2025-10-23 00:00:00'),(197,29,1,'2025-10-29 00:00:00'),(198,29,1,'2025-10-28 00:00:00'),(199,29,1,'2025-10-27 00:00:00'),(200,29,1,'2025-10-26 00:00:00'),(201,29,0,'2025-10-25 00:00:00'),(202,29,1,'2025-10-24 00:00:00'),(203,29,0,'2025-10-23 00:00:00'),(204,30,1,'2025-10-29 00:00:00'),(205,30,1,'2025-10-28 00:00:00'),(206,30,1,'2025-10-27 00:00:00'),(207,30,1,'2025-10-26 00:00:00'),(208,30,0,'2025-10-25 00:00:00'),(209,30,1,'2025-10-24 00:00:00'),(210,30,0,'2025-10-23 00:00:00'),(211,5,1,'2025-10-31 00:00:00'),(212,25,1,'2025-10-31 00:00:00'),(213,2,3,'2025-10-31 00:00:00'),(214,3,1,'2025-10-31 00:00:00'),(215,4,1,'2025-10-31 00:00:00'),(216,4,3,'2025-11-02 00:00:00'),(217,2,6,'2025-11-02 00:00:00'),(218,3,1,'2025-11-02 00:00:00'),(219,10,2,'2025-11-02 00:00:00'),(220,4,10,'2025-11-03 00:00:00'),(221,3,2,'2025-11-03 00:00:00'),(222,5,2,'2025-11-03 00:00:00'),(223,2,2,'2025-11-03 00:00:00'),(224,1,3,'2025-11-03 00:00:00'),(225,2,1,'2025-11-06'),(226,17,1,'2025-11-06'),(227,5,1,'2025-11-06'),(228,8,1,'2025-11-06'),(229,27,1,'2025-11-06'),(230,14,1,'2025-11-06'),(231,26,1,'2025-11-06'),(232,28,1,'2025-11-08'),(233,3,1,'2025-11-08'),(234,2,2,'2025-11-08'),(235,26,1,'2025-11-08'),(236,15,1,'2025-11-09'),(237,18,1,'2025-11-09'),(238,4,1,'2025-11-09'),(239,3,2,'2025-11-10'),(240,7,1,'2025-11-10'),(241,22,1,'2025-11-10'),(242,8,1,'2025-11-10'),(243,9,1,'2025-11-10'),(244,5,1,'2025-11-10'),(245,3,1,'2025-11-11'),(246,4,1,'2025-11-13'),(247,13,1,'2025-11-13'),(248,30,1,'2025-11-13'),(249,5,3,'2025-11-14'),(250,3,3,'2025-11-14'),(251,1,3,'2025-11-14'),(252,26,1,'2025-11-14'),(253,29,1,'2025-11-14'),(254,28,2,'2025-11-14'),(255,10,2,'2025-11-14'),(256,23,1,'2025-11-14'),(257,25,2,'2025-11-14'),(258,27,1,'2025-11-14'),(259,20,1,'2025-11-14'),(260,19,1,'2025-11-14'),(261,24,1,'2025-11-14'),(262,21,2,'2025-11-14'),(263,22,1,'2025-11-14');
/*!40000 ALTER TABLE `bookreads` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `books`
--

DROP TABLE IF EXISTS `books`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `books` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Author` text DEFAULT NULL,
  `Category` text DEFAULT NULL,
  `CoverImagePath` text DEFAULT NULL,
  `Description` text DEFAULT NULL,
  `FilePath` text DEFAULT NULL,
  `ISBN` text DEFAULT NULL,
  `NumberOfReads` bigint(20) DEFAULT NULL,
  `Title` text DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `books`
--

LOCK TABLES `books` WRITE;
/*!40000 ALTER TABLE `books` DISABLE KEYS */;
INSERT INTO `books` VALUES (1,'Eric Matthes','Programming','/assets/img/covers/python-crash-course-cover.jpg','A hands-on, project-based introduction to programming with Python.','/assets/books/python-crash-course.pdf','9781593279288',952,'Python Crash Course'),(2,'James Clear','Self-Help','/assets/img/covers/atomic-habits-cover.jpg','An easy & proven way to build good habits and break bad ones.','/assets/books/atomic-habits.pdf','9780735211292',1002,'Atomic Habits'),(3,'Matt Haig','Fiction','/assets/img/covers/the-midnight-library-cover.jpg','A woman finds herself in a library between life and death, where every book represents a different life she could have lived.','/assets/books/the-midnight-library.pdf','9780525559474',903,'The Midnight Library'),(4,'Joel Grus','Programming','/assets/img/covers/data-science-from-scratch-cover.jpg','Learn data science fundamentals with Python, including statistics, machine learning, and data visualization.','/assets/books/data-science-from-scratch.pdf','9781492041139',876,'Data Science from Scratch'),(5,'Hector Garcia & Francesc Miralles','Self-Help','/assets/img/covers/ikigai-cover.jpg','The Japanese secret to a long and happy life.','/assets/books/ikigai.pdf','9780143130727',834,'Ikigai'),(6,'Thomas H. Cormen','Programming','/assets/img/covers/introduction-to-algorithms-cover.jpg','A comprehensive introduction to algorithms and their analysis.','/assets/books/introduction-to-algorithms.pdf','9780262033848',413,'Introduction to Algorithms'),(7,'Ryan Holiday','Self-Help','/assets/img/covers/ego-is-the-enemy-cover.jpg','A powerful exploration of how ego can be the biggest obstacle to success.','/assets/books/ego-is-the-enemy.pdf','9781591846352',280,'Ego is the Enemy'),(8,'Brian Christian & Tom Griffiths','Self-Help','/assets/img/covers/algorithms-to-live-by-cover.jpg','A fascinating exploration of how computer algorithms can be applied to our everyday lives.','/assets/books/algorithms-to-live-by.pdf','9781627790369',237,'Algorithms to Live By'),(9,'Brianna Wiest','Self-Help','/assets/img/covers/the-mountain-is-you-cover.jpg','A transformative guide to self-sabotage and how to overcome it.','/assets/books/the-mountain-is-you.pdf','9781949759222',169,'The Mountain is You'),(10,'Paul Kleinman','Non-Fiction','/assets/img/covers/philosophy-101-cover.jpg','From Plato and Socrates to ethics and metaphysics, an essential primer on philosophical thought.','/assets/books/philosophy-101.pdf','9781440541763',917,'Philosophy 101'),(11,'Bianca Sparacino','Self-Help','/assets/img/covers/a-gentle-reminder-cover.jpg','A collection of poetry and prose about healing, growth, and self-discovery.','/assets/books/a-gentle-reminder.pdf','9781734447000',190,'A Gentle Reminder'),(12,'Morgan Housel','Business','/assets/img/covers/the-psychology-of-money-cover.jpg','Timeless lessons on wealth, greed, and happiness.','/assets/books/the-psychology-of-money.pdf','9780857197689',791,'The Psychology of Money'),(13,'Andrew Trask','Programming','/assets/img/covers/grokking-deep-learning-cover.jpg','A practical guide to deep learning that teaches you how to build neural networks from scratch.','/assets/books/grokking-deep-learning.pdf','9781617293702',291,'Grokking Deep Learning'),(14,'Paulo Coelho','Fiction','/assets/img/covers/the-alchemist-cover.jpg','A philosophical novel about following your dreams and listening to your heart.','/assets/books/the-alchemist.pdf','9780062315007',792,'The Alchemist'),(15,'Yuval Noah Harari','Non-Fiction','/assets/img/covers/sapiens-cover.jpg','A brief history of humankind from ancient humans to the present.','/assets/books/sapiens.pdf','9780062316097',569,'Sapiens'),(16,'Robert T. Kiyosaki','Business','/assets/img/covers/rich-dad-poor-dad-cover.jpg','What the rich teach their kids about money that the poor and middle class do not.','/assets/books/rich-dad-poor-dad.pdf','9781612680194',894,'Rich Dad Poor Dad'),(17,'Peter Thiel','Business','/assets/img/covers/zero-to-one-cover.jpg','Notes on startups, or how to build the future.','/assets/books/zero-to-one.pdf','9780804139298',347,'Zero to One'),(18,'Napoleon Hill','Business','/assets/img/covers/think-and-grow-rich-cover.jpg','A classic guide to personal achievement and wealth building.','/assets/books/think-and-grow-rich.pdf','9781585424337',791,'Think and Grow Rich'),(19,'Gayle Laakmann McDowell','Programming','/assets/img/covers/cracking-the-coding-interview-cover.jpg','A comprehensive guide to technical interviews with 189 programming questions and solutions.','/assets/books/cracking-the-coding-interview.pdf','9780984782857',680,'Cracking the Coding Interview'),(20,'Andrew Lock','Programming','/assets/img/covers/asp-net-core-in-action-cover.jpg','A comprehensive guide to building web applications with ASP.NET Core.','/assets/books/asp-net-core-in-action.pdf','9781617294617',180,'ASP.NET Core in Action'),(21,'David Baldacci','Fiction','/assets/img/covers/memory-man-cover.jpg','A thrilling mystery featuring Amos Decker, a detective with a perfect memory.','/assets/books/memory-man.pdf','9781455586384',316,'Memory Man'),(22,'J.R.R. Tolkien','Fiction','/assets/img/covers/the-hobbit-cover.jpg','A young hobbit\'s journey to win a treasure guarded by a dragon, and the friends he makes along the way.','/assets/books/the-hobbit.pdf','9780345339683',657,'The Hobbit'),(23,'Darren Hardy','Business','/assets/img/covers/the-compound-effect-cover.jpg','A practical guide to achieving success through small, consistent actions.','/assets/books/the-compound-effect.pdf','9781593157241',414,'The Compound Effect'),(24,'Benjamin Graham','Business','/assets/img/covers/the-intelligent-investor-cover.jpg','The definitive book on value investing.','/assets/books/the-intelligent-investor.pdf','9780060555665',680,'The Intelligent Investor'),(25,'Sun Tzu','Non-Fiction','/assets/img/covers/art-of-war-cover.jpg','An ancient Chinese military treatise on strategy and tactics.','/assets/books/the-art-of-war.pdf','9780140439199',461,'The Art of War'),(26,'Robert Greene','Non-Fiction','/assets/img/covers/the-48-laws-of-power-cover.jpg','A practical guide to understanding power dynamics and human nature.','/assets/books/the-48-laws-of-power.pdf','9780140280197',793,'48 Laws of Power'),(27,'Arthur Conan Doyle','Fiction','/assets/img/covers/sherlock-holmes-cover.jpg','The complete collection of Sherlock Holmes stories featuring the world\'s most famous detective.','/assets/books/sherlock-holmes.pdf','9780140439070',570,'Sherlock Holmes'),(28,'Viktor E. Frankl','Non-Fiction','/assets/img/covers/man-search-for-meaning-cover.jpg','A psychiatrist\'s personal experience of life in Nazi concentration camps and the search for meaning in suffering.','/assets/books/man-search-for-meaning.pdf','9780807014295',246,'Man\'s Search for Meaning'),(29,'Antoine de Saint-Exupéry','Fiction','/assets/img/covers/the-little-prince-cover.jpg','A poetic tale about a young prince who visits various planets in space.','/assets/books/the-little-prince.pdf','9780156013987',458,'The Little Prince'),(30,'DK','Non-Fiction','/assets/img/covers/the-history-book-cover.jpg','A comprehensive guide to world history from ancient times to the present day.','/assets/books/the-history-book.pdf','9781465445100',236,'The History Book');
/*!40000 ALTER TABLE `books` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `comments`
--

DROP TABLE IF EXISTS `comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `comments` (
  `CommentId` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserId` bigint(20) DEFAULT NULL,
  `BookId` bigint(20) DEFAULT NULL,
  `CommentText` text DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `IsHidden` tinyint(1) DEFAULT 0,
  PRIMARY KEY (`CommentId`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comments`
--

LOCK TABLES `comments` WRITE;
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
INSERT INTO `comments` VALUES (2,17,3,'Good Book!!','2025-11-09 16:13:31',0),(3,17,3,'Good Book!!','2025-11-09 16:21:29',0),(4,17,3,'kghkkjjkhkkj','2025-11-09 16:27:54',0),(5,17,3,'gfdsgfgbfd','2025-11-09 16:38:45',0),(6,17,3,'Good Book!!','2025-11-09 16:40:38',0),(7,17,22,'Good Book!!','2025-11-09 17:27:17',0),(8,17,12,'fbgfdfdg','2025-11-09 17:32:13',0),(9,17,3,'fdsfdsfdsf','2025-11-09 17:36:34',0),(10,17,3,'wdad','2025-11-09 17:40:12',0),(11,17,3,'awd','2025-11-09 17:58:02',0),(12,17,4,'grdgdg','2025-11-09 18:06:29',0),(13,17,2,'dada','2025-11-10 13:42:26',0),(14,17,3,'csaasd','2025-11-11 15:49:00',0),(15,17,4,'helloo','2025-11-13 09:53:51',0),(16,3,29,'ewfwe','2025-11-13 14:45:10',0),(17,17,28,'dwad','2025-11-13 15:09:25',0),(18,17,30,'gfg','2025-11-13 15:33:18',0),(19,17,30,'fsdsf','2025-11-13 15:38:35',0),(20,17,9,'thrthrt','2025-11-13 15:51:16',0),(21,17,3,'hello','2025-11-13 16:02:29',0),(22,17,5,'fsefsf','2025-11-13 16:03:14',0),(23,17,2,'dsfdf','2025-11-13 17:24:58',0),(24,17,2,'dsfdf','2025-11-13 17:24:58',0),(25,17,2,'fdfds','2025-11-13 17:25:05',0),(26,17,26,'hello','2025-11-13 17:26:10',0),(27,3,29,'jheris','2025-11-13 17:27:11',0),(28,17,23,'gdgd','2025-11-14 01:23:08',0),(29,17,21,'hello','2025-11-14 03:32:51',0);
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `favorites`
--

DROP TABLE IF EXISTS `favorites`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `favorites` (
  `FavoriteId` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserId` bigint(20) DEFAULT NULL,
  `BookId` bigint(20) DEFAULT NULL,
  `CreatedAt` text DEFAULT NULL,
  PRIMARY KEY (`FavoriteId`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `favorites`
--

LOCK TABLES `favorites` WRITE;
/*!40000 ALTER TABLE `favorites` DISABLE KEYS */;
INSERT INTO `favorites` VALUES (4,2,1,'2025-11-09 21:41:20'),(6,17,28,'2025-11-09 13:43:15'),(8,17,22,'2025-11-09 17:27:09'),(12,3,27,'2025-11-14 03:38:32');
/*!40000 ALTER TABLE `favorites` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userroles`
--

DROP TABLE IF EXISTS `userroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userroles` (
  `RoleID` int(11) NOT NULL,
  `RoleName` varchar(100) NOT NULL,
  PRIMARY KEY (`RoleID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userroles`
--

LOCK TABLES `userroles` WRITE;
/*!40000 ALTER TABLE `userroles` DISABLE KEYS */;
INSERT INTO `userroles` VALUES (1,'Admin'),(2,'User');
/*!40000 ALTER TABLE `userroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `AvatarPath` text DEFAULT NULL,
  `Password` text DEFAULT NULL,
  `Role` text DEFAULT NULL,
  `UserName` text DEFAULT NULL,
  `Email` text DEFAULT NULL,
  `FirstName` text DEFAULT NULL,
  `LastName` text DEFAULT NULL,
  `RoleID` int(11) NOT NULL DEFAULT 2,
  `StatusID` int(11) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`),
  KEY `IX_users_RoleID` (`RoleID`),
  KEY `IX_users_StatusID` (`StatusID`),
  CONSTRAINT `FK_users_UserRoles_RoleID` FOREIGN KEY (`RoleID`) REFERENCES `userroles` (`RoleID`) ON UPDATE CASCADE,
  CONSTRAINT `FK_users_UserStatuses_StatusID` FOREIGN KEY (`StatusID`) REFERENCES `userstatuses` (`StatusID`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,NULL,'$2y$10$9uxhjY.fNBKRl5R/xl6tfexIgJGgSNfeEJXF3tQizpE9eL6.iyV.O','Admin','Admin','','','',1,1),(2,NULL,'$2y$10$X.6yVKAmwB57g9SI9sE2ouEKHiIogf1.aaEfK1alVR20Oa51.VUf2','User','nicko_712','','','',2,1),(3,NULL,'$2y$10$9L0OOodPsbYGpR2g42HDf.PUIeLPuEAGf3doUuMslUaXLOwBShkM2','User','jheris456','','','',2,1),(4,NULL,'$2y$10$mAoUz/qaVKIvvpHvHrl1CeOlXoWEmbQOJsK24rT7I3e/ZDGCF9Xja','User','reinald.42','','','',2,1),(5,NULL,'$2y$10$en1inGogPew7cz7bl4Q5Lea32ZU7pon7Sua2KpniEkwR6I8OnVRka','User','pythonic712','','','',2,1),(6,NULL,'$2y$10$Dia/6t36GUxtpC6rhiZTVOdVMTBQsewZUIXSmpvjrJp.aWe6Il.2W','User','tech_guru_99','','','',2,1),(7,NULL,'$2y$10$DUmrZYPBLSPyt4l4o8rfquHSA6TXHFu7ha5UroZL.a7GcLYqUj09q','User','coderx_77','','','',2,1),(8,NULL,'$2y$10$Agz4Wozw2j2kh1NYK55oHufg5ZUb4myI6jfnUhwmprfdI611RWxmS','User','skywalker001','','','',2,1),(9,NULL,'$2y$10$nm/c2DKS/vnr182XO5KrrOC7W8MpXcHJ1FEwAKnrVctQnyjT2Ljqu','User','dreamer_88','','','',2,1),(10,NULL,'$2y$10$5Z7RpwwDCvFu01j5suV5cuKxh3RwM59eb8odBaot3sjFU./e0IIB2','User','pixelwizard_12','','','',1,1),(11,NULL,'$2y$10$HAiaUCUc621xvjF9tZF4iuQ5unu8gcWBju7cRwGibfzQqZY6N.4De','User','ninja_42','','','',1,1),(12,NULL,'$2y$10$QdqPzNHcXMs9DDCoQ7btbeFM4l.H4q3V4eY28T16hV5b37F.441qO','User','binarybee99','','','',1,1),(13,NULL,'$2y$10$ADYFrkOLen3SINruUrd6bu2.2WzncTmFzbmE6Sm/9sG4X779REOmy','User','cloudybrain','','','',1,1),(14,NULL,'$2y$10$EOE2GGSueYhRFPsELotqneTMYGESbGcrmG9ZggjBPTFqGf842wJOC','User','gadgetqueen','','','',1,1),(15,NULL,'$2y$10$uLRhWZffksBRUx1Kuw8I4uzVXnS.A6HqjQ.x6.nOLcymEF90VERLG','User','bytebrawler','','','',1,1),(16,NULL,'$2y$10$BZfx0szsmDSUU.x0jjs4luf.PeVa45IJsqCCKQzoT73yolVQdJuvS','User','cosmicdevv','','','',1,1),(17,NULL,'$2y$10$vNREh77HhKHy0zgQx4uFQ.3yMKLG55RPX623oPaLnHPQOd7OtpMTG','User','Venus','venusirish25@gmail.com','Venus Irish','Lagrimas',1,1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userstatuses`
--

DROP TABLE IF EXISTS `userstatuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userstatuses` (
  `StatusID` int(11) NOT NULL,
  `StatusName` varchar(100) NOT NULL,
  PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userstatuses`
--

LOCK TABLES `userstatuses` WRITE;
/*!40000 ALTER TABLE `userstatuses` DISABLE KEYS */;
INSERT INTO `userstatuses` VALUES (0,'Inactive'),(1,'Active');
/*!40000 ALTER TABLE `userstatuses` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-11-17 23:22:24
