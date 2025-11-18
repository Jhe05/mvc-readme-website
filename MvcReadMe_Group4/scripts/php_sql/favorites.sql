-- favorites.sql
-- Creates a `favorites` table in MySQL suitable for phpMyAdmin import.
-- Adjust the referenced table names (users, books) if your schema uses different names.

CREATE TABLE IF NOT EXISTS `favorites` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_id` INT UNSIGNED NOT NULL,
  `book_id` INT UNSIGNED NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `idx_favorites_user_id` (`user_id`),
  INDEX `idx_favorites_book_id` (`book_id`),
  CONSTRAINT `fk_favorites_user` FOREIGN KEY (`user_id`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_favorites_book` FOREIGN KEY (`book_id`) REFERENCES `books`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Example: insert a sample favorite (uncomment to run)
-- INSERT INTO `favorites` (`user_id`, `book_id`) VALUES (1, 1);
