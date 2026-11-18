-- Тестовые данные для пустой базы pepedb.
-- Сначала импортируйте файл «pepe (2).sql», потом этот.
-- Второй раз лучше не запускать: категории и товары задвоятся.
SET NAMES utf8mb4;
USE pepedb;

INSERT INTO categories (name) VALUES
('Кепки'),
('Футболки'),
('Аксессуары');

INSERT INTO products (title, image, description, cost, count, categoryId, status) VALUES
('PEPKA CAP', 'cap.svg', 'Кепка с принтом Pepe. Удобная и яркая.', 1500.00, 12, 1, 'Активен'),
('GREEN CAP', 'product.svg', 'Зелёная кепка из коллекции CopeNothing.', 1700.00, 7, 1, 'Активен'),
('PEPE TEE', 'tshirt.svg', 'Футболка с классическим Pepe. Хлопок, свободный крой.', 2200.00, 10, 2, 'Активен'),
('PNDC TEE', 'cover.svg', 'Футболка с принтом $PNDC.', 1900.00, 8, 2, 'Активен'),
('PEEP SHIRT', 'peep.svg', 'Футболка с лягушонком. Лимитированный тираж.', 2100.00, 5, 2, 'Активен'),
('SMILE PEPE', 'pepe-smile.svg', 'Маленький значок-улыбка. Можно повесить на рюкзак.', 900.00, 20, 3, 'Активен'),
('CLASSIC PEPE', 'pepe1.svg', 'Постер с классическим Pepe.', 1200.00, 15, 3, 'Активен'),
('POND SET', 'pepe2.svg', 'Набор стикеров Pond и Pepe.', 2500.00, 6, 3, 'Активен');

INSERT INTO users (surname, name, patronymic, status, role, login, password) VALUES
('Петров', 'Иван', 'Сергеевич', 'Активен', 'Клиент', 'pepe', '12345'),
('Админов', 'Админ', '', 'Активен', 'Админ', 'admin', 'admin');
