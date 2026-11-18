create database if not exists pepedb;
use pepedb;

-- 1. Таблица пользователей
create table if not exists users(
    id int auto_increment primary key,
    surname varchar(100) not null,
    name varchar(100) not null,
    patronymic varchar(100), 
    status enum("Удалён", "Активен") not null default "Активен",
    role enum("Админ", "Клиент") not null default "Клиент",
    login varchar(20) not null,
    password varchar(20) not null
);

-- 2. Таблица категорий
create table if not exists categories(
    id int auto_increment primary key,
    name varchar(100) not null
);

-- 3. Таблица товаров
create table if not exists products(
    id int auto_increment primary key,
    title varchar(100) not null,
    image varchar(255) not null,
    description text not null,
    cost decimal(10, 2) not null,
    count int not null default 0,
    categoryId int not null,
    status enum("Активен", "Удалён") default "Активен",

    constraint fk_product_category foreign key (categoryId) references categories(id)
    on delete cascade );

create table if not exists orders(
    id int auto_increment primary key,
    priceAll decimal(10, 2) not null,
    dateOrder datetime default current_timestamp,
    userId int not null,
    status enum("Новый", "Оплачен", "Доставлен", "Отменен") default "Новый",
    addressDelivery text not null,
    dateDelivery date not null,

    constraint fk_order_user foreign key (userId) references users(id)
);


create table if not exists orderItem(
    id int auto_increment primary key,
    order_id int not null,
    product_id int not null,
    count int not null default 1,

    constraint fk_item_order foreign key (order_id) references orders(id) on delete cascade,
    constraint fk_item_product foreign key (product_id) references products(id)
);

create table if not exists basket(
    id int auto_increment primary key,
    user_id int not null,
    product_id int not null,
    count int not null,

    constraint fk_item_user2 foreign key (user_id) references users(id)
)