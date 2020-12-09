-- phpMyAdmin SQL Dump
-- version 4.9.0.1
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Дек 09 2020 г., 21:16
-- Версия сервера: 10.3.13-MariaDB-log
-- Версия PHP: 7.1.32

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `chatdb`
--

-- --------------------------------------------------------

--
-- Структура таблицы `history`
--

CREATE TABLE `history` (
  `id` int(11) NOT NULL,
  `fromId` int(11) NOT NULL,
  `toid` int(11) NOT NULL,
  `name` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `message` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `history`
--

INSERT INTO `history` (`id`, `fromId`, `toid`, `name`, `message`) VALUES
(1, 1, 0, 'test', '<myID>2'),
(2, 1, 0, 'test', '123'),
(3, 1, 1, 'test', '456'),
(4, 1, 0, 'Игорь', '<myID>2'),
(5, 1, 0, 'Игорь', '12341'),
(6, 2, 0, 'Игорь', '12312'),
(7, 2, 0, 'Игорь', '123'),
(8, 2, 0, 'Игорь', '43'),
(9, 2, 0, 'Игорь', '555'),
(10, 2, 2, 'Игорь', '1'),
(11, 2, 2, 'Игорь', '2'),
(12, 2, 0, 'Игорь', 'retert'),
(13, 2, 0, 'Игорь', '12312'),
(14, 2, 2, 'Игорь', '12412'),
(15, 1, 0, 'Игорь', '124124'),
(16, 1, 0, 'Игорь', '5235'),
(17, 1, 0, 'Игорь', 'qweqw'),
(18, 1, 0, 'Игорь', 'qweq'),
(19, 1, 0, 'Игорь', 'tew'),
(20, 1, 1, 'Игорь', 'werw'),
(21, 1, 0, 'Игорь', 'werw');

-- --------------------------------------------------------

--
-- Структура таблицы `logins`
--

CREATE TABLE `logins` (
  `id` int(11) NOT NULL,
  `user` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `password` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `logins`
--

INSERT INTO `logins` (`id`, `user`, `password`) VALUES
(1, 'admin', 'admin'),
(2, 'test', 'test');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `history`
--
ALTER TABLE `history`
  ADD PRIMARY KEY (`id`);

--
-- Индексы таблицы `logins`
--
ALTER TABLE `logins`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `unique` (`user`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `history`
--
ALTER TABLE `history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT для таблицы `logins`
--
ALTER TABLE `logins`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
