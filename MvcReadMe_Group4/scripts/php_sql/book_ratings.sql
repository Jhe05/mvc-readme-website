-- book_ratings.sql
-- Creates the book_ratings table and admin_notifications table for MySQL/phpMyAdmin

CREATE TABLE IF NOT EXISTS `book_ratings` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `book_id` bigint(20) NOT NULL,
  `user_id` bigint(20) NOT NULL,
  `rating` TINYINT NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `ux_book_user` (`book_id`, `user_id`),
  INDEX `idx_book_id` (`book_id`),
  INDEX `idx_user_id` (`user_id`),
  CONSTRAINT `fk_bookratings_book` FOREIGN KEY (`book_id`) REFERENCES `books`(`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_bookratings_user` FOREIGN KEY (`user_id`) REFERENCES `users`(`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Simple notifications table for admin alerts
CREATE TABLE IF NOT EXISTS `admin_notifications` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `message` TEXT NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Example insert (uncomment to run)
-- INSERT INTO `book_ratings` (`book_id`, `user_id`, `rating`) VALUES (1, 1, 5);
