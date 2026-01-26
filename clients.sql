-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Jan 26. 19:37
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `fitness_app`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `clients`
--

CREATE TABLE `clients` (
  `client_id` int(10) UNSIGNED NOT NULL,
  `full_name` varchar(120) NOT NULL,
  `email` varchar(180) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `phone` varchar(30) DEFAULT NULL,
  `date_of_birth` date DEFAULT NULL,
  `height_cm` smallint(5) UNSIGNED DEFAULT NULL,
  `weight_kg` decimal(5,2) DEFAULT NULL,
  `role` enum('client','admin') NOT NULL DEFAULT 'client',
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `is_active` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `clients`
--

INSERT INTO `clients` (`client_id`, `full_name`, `email`, `password_hash`, `phone`, `date_of_birth`, `height_cm`, `weight_kg`, `role`, `created_at`, `updated_at`, `is_active`) VALUES
(1, 'Teszt Elek', 'elek@demo.local', '$2a$11$DUMMYHASH_ELEK', '+36 70 100 0001', '2005-03-14', 178, 78.50, 'client', '2026-01-26 17:55:36', '2026-01-26 17:55:36', 1),
(2, 'Kiss Anna', 'anna@demo.local', '$2a$11$DUMMYHASH_ANNA', '+36 70 100 0002', '2004-10-02', 165, 60.20, 'client', '2026-01-26 17:55:36', '2026-01-26 17:55:36', 1),
(3, 'Nagy Dávid', 'david@demo.local', '$2a$11$DUMMYHASH_DAVID', '+36 70 100 0003', '2003-06-21', 182, 86.10, 'client', '2026-01-26 17:55:36', '2026-01-26 17:55:36', 1),
(4, 'Admin User', 'admin@demo.local', '$2a$11$DUMMYHASH_ADMIN', '+36 70 100 9999', '2000-01-01', 180, 80.00, 'admin', '2026-01-26 17:55:36', '2026-01-26 17:55:36', 1);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `clients`
--
ALTER TABLE `clients`
  ADD PRIMARY KEY (`client_id`),
  ADD UNIQUE KEY `uq_clients_email` (`email`),
  ADD KEY `idx_clients_role` (`role`),
  ADD KEY `idx_clients_active` (`is_active`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `clients`
--
ALTER TABLE `clients`
  MODIFY `client_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
