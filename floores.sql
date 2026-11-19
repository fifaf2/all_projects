-- База данных для api_floower (соответствует FlooresContext)
CREATE DATABASE IF NOT EXISTS `floores`
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE `floores`;

-- Роли
CREATE TABLE `roles` (
  `id_role`   INT NOT NULL,
  `name_role` VARCHAR(100) NULL,
  PRIMARY KEY (`id_role`)
) ENGINE=InnoDB;

-- Статусы заказов
CREATE TABLE `statuses` (
  `id_status`   INT NOT NULL,
  `name_status` VARCHAR(45) NULL,
  PRIMARY KEY (`id_status`)
) ENGINE=InnoDB;

-- Поставщики
CREATE TABLE `postavshik` (
  `id_post`   INT NOT NULL,
  `name_post` VARCHAR(405) NULL,
  PRIMARY KEY (`id_post`)
) ENGINE=InnoDB;

-- Пользователи
CREATE TABLE `users` (
  `id_user`  INT NOT NULL,
  `FIO`      VARCHAR(45) NULL,
  `login`    VARCHAR(45) NULL,
  `password` VARCHAR(45) NULL,
  `id_role`  INT NULL,
  PRIMARY KEY (`id_user`),
  INDEX `ld_idx` (`id_role`),
  CONSTRAINT `ld` FOREIGN KEY (`id_role`) REFERENCES `roles` (`id_role`)
) ENGINE=InnoDB;

-- Товары
CREATE TABLE `products` (
  `id_product`   INT NOT NULL,
  `name_product` VARCHAR(405) NULL,
  `id_post`      INT NULL,
  `sell`         INT NULL,
  `kolvo`        INT NULL,
  `description`  VARCHAR(405) NULL,
  PRIMARY KEY (`id_product`),
  INDEX `as_idx` (`id_post`),
  CONSTRAINT `as` FOREIGN KEY (`id_post`) REFERENCES `postavshik` (`id_post`)
) ENGINE=InnoDB;

-- Заказы
CREATE TABLE `orders` (
  `id_order`   INT NOT NULL,
  `id_user`    INT NULL,
  `data_zakaz` DATETIME NULL,
  `code_poly`  INT NULL,
  `id_status`  INT NULL,
  PRIMARY KEY (`id_order`),
  INDEX `a_idx` (`id_user`),
  INDEX `b_idx` (`id_status`),
  CONSTRAINT `a` FOREIGN KEY (`id_user`)   REFERENCES `users` (`id_user`),
  CONSTRAINT `b` FOREIGN KEY (`id_status`) REFERENCES `statuses` (`id_status`)
) ENGINE=InnoDB;

-- Состав заказов
CREATE TABLE `productorders` (
  `id_productorders` INT NOT NULL,
  `id_product`       INT NULL,
  `kolvo`            INT NULL,
  `id_order`         INT NULL,
  PRIMARY KEY (`id_productorders`),
  INDEX `a_idx` (`id_product`),
  INDEX `b_idx` (`id_order`),
  CONSTRAINT `ab` FOREIGN KEY (`id_product`) REFERENCES `products` (`id_product`),
  CONSTRAINT `df` FOREIGN KEY (`id_order`)   REFERENCES `orders` (`id_order`)
) ENGINE=InnoDB;

-- ============================================================
-- Тестовые данные
-- ============================================================

INSERT INTO `roles` (`id_role`, `name_role`) VALUES
  (1, 'Администратор'),
  (2, 'Менеджер'),
  (3, 'Клиент');

INSERT INTO `statuses` (`id_status`, `name_status`) VALUES
  (1, 'Новый'),
  (2, 'В обработке'),
  (3, 'Собран'),
  (4, 'Доставляется'),
  (5, 'Выполнен'),
  (6, 'Отменён');

INSERT INTO `postavshik` (`id_post`, `name_post`) VALUES
  (1, 'ООО "Цветочная база"'),
  (2, 'Flora Holland'),
  (3, 'ИП Садовников А.В.'),
  (4, 'Тепличный комплекс "Заря"');

INSERT INTO `users` (`id_user`, `FIO`, `login`, `password`, `id_role`) VALUES
  (1, 'Иванов Иван Иванович',     'admin',    'admin123',  1),
  (2, 'Петрова Мария Сергеевна',  'manager',  'manager123', 2),
  (3, 'Сидоров Алексей Петрович', 'client1',  'client123', 3),
  (4, 'Козлова Анна Викторовна',  'client2',  'client123', 3),
  (5, 'Смирнов Дмитрий Олегович', 'client3',  'client123', 3);

INSERT INTO `products` (`id_product`, `name_product`, `id_post`, `sell`, `kolvo`, `description`) VALUES
  (1,  'Роза красная 60 см',        2, 150,  200, 'Классическая красная роза, длина стебля 60 см'),
  (2,  'Роза белая 50 см',          2, 140,  150, 'Белая роза, длина стебля 50 см'),
  (3,  'Тюльпан жёлтый',            4,  80,  300, 'Жёлтый тюльпан, сорт Strong Gold'),
  (4,  'Тюльпан розовый',           4,  80,  250, 'Нежно-розовый тюльпан'),
  (5,  'Хризантема кустовая',       1, 120,  100, 'Кустовая хризантема, разные цвета'),
  (6,  'Лилия белая',               1, 210,   80, 'Крупная белая лилия с ароматом'),
  (7,  'Гербера микс',              3,  95,  180, 'Гербера, смесь ярких цветов'),
  (8,  'Орхидея фаленопсис',        2, 1800,  25, 'Орхидея в горшке, 2 цветоноса'),
  (9,  'Пион розовый',              2, 250,   60, 'Сезонный пион, крупный бутон'),
  (10, 'Букет "Нежность"',          1, 2500,  15, 'Букет из роз, эустомы и зелени'),
  (11, 'Букет "Весна"',             4, 1900,  20, 'Букет из тюльпанов и нарциссов'),
  (12, 'Фикус Бенджамина',          3, 1200,  30, 'Комнатное растение в горшке, высота 60 см');

INSERT INTO `orders` (`id_order`, `id_user`, `data_zakaz`, `code_poly`, `id_status`) VALUES
  (1, 3, '2026-11-01 10:15:00', 1001, 5),
  (2, 4, '2026-11-05 14:30:00', 1002, 5),
  (3, 3, '2026-11-12 09:45:00', 1003, 4),
  (4, 5, '2026-11-15 18:20:00', 1004, 3),
  (5, 4, '2026-11-17 12:00:00', 1005, 2),
  (6, 5, '2026-11-18 16:10:00', 1006, 1),
  (7, 3, '2026-11-19 11:05:00', 1007, 6);

INSERT INTO `productorders` (`id_productorders`, `id_product`, `kolvo`, `id_order`) VALUES
  (1,  1,  11, 1),
  (2,  6,   3, 1),
  (3,  10,  1, 2),
  (4,  3,  15, 3),
  (5,  4,  15, 3),
  (6,  8,   1, 4),
  (7,  9,   5, 5),
  (8,  2,   9, 5),
  (9,  12,  1, 6),
  (10, 7,  10, 7),
  (11, 11,  1, 7);
