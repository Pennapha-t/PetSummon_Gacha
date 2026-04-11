CREATE TABLE IF NOT EXISTS Player (
player_id INTEGER PRIMARY KEY AUTOINCREMENT,
username  TEXT NOT NULL UNIQUE,
gold_coin INTEGER NOT NULL DEFAULT 0,
diamond INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS Pet (
pet_id INTEGER PRIMARY KEY AUTOINCREMENT,
pet_name TEXT NOT NULL,
rarity INTEGER NOT NULL,
element TEXT,
role TEXT,
base_attack INTEGER NOT NULL,
base_defense INTEGER NOT NULL,
base_hp INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS Player_Pet (
player_pet_id INTEGER PRIMARY KEY AUTOINCREMENT,
player_id INTEGER NOT NULL,
pet_id INTEGER NOT NULL,
level INTEGER NOT NULL DEFAULT 1,
exp INTEGER NOT NULL DEFAULT 0,
is_active INTEGER NOT NULL DEFAULT 0,
FOREIGN KEY (player_id) REFERENCES Player(player_id),
FOREIGN KEY (pet_id) REFERENCES Pet(pet_id)
);

CREATE TABLE IF NOT EXISTS Monster (
monster_id INTEGER PRIMARY KEY AUTOINCREMENT,
name TEXT NOT NULL,
element TEXT,
hp INTEGER NOT NULL,
attack INTEGER NOT NULL,
defense INTEGER NOT NULL,
reward_exp INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS Monster_Set (
set_id INTEGER PRIMARY KEY AUTOINCREMENT,
monster_id INTEGER NOT NULL,
stage_order INTEGER NOT NULL,
FOREIGN KEY (monster_id) REFERENCES Monster(monster_id)
);

CREATE TABLE IF NOT EXISTS Stage (
stage_id INTEGER PRIMARY KEY AUTOINCREMENT,
stage_name TEXT NOT NULL,
difficulty INTEGER NOT NULL,
reward_currency INTEGER NOT NULL,  
reward_amount INTEGER NOT NULL,
monster_set_id INTEGER NOT NULL,
FOREIGN KEY (reward_currency) REFERENCES Currency(currency_id),
FOREIGN KEY (monster_set_id) REFERENCES Monster_Set(set_id)
);

CREATE TABLE IF NOT EXISTS Currency (
currency_id INTEGER PRIMARY KEY AUTOINCREMENT,
currency_name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Item (
item_id INTEGER PRIMARY KEY AUTOINCREMENT,
item_name TEXT NOT NULL,
item_type TEXT NOT NULL,
rarity INTEGER, 
description TEXT
);

CREATE TABLE IF NOT EXISTS Shop (
shop_id INTEGER PRIMARY KEY AUTOINCREMENT,
shop_name TEXT NOT NULL,
shop_type TEXT NOT NULL,
description TEXT
);

CREATE TABLE IF NOT EXISTS Shop_Item (
shop_item_id INTEGER PRIMARY KEY AUTOINCREMENT,
shop_id INTEGER,
item_id INTEGER NOT NULL,       
price_currency_id INTEGER NOT NULL,       
price_amount INTEGER NOT NULL,
FOREIGN KEY (shop_id) REFERENCES Shop(shop_id),
FOREIGN KEY (item_id) REFERENCES Item(item_id),
FOREIGN KEY (price_currency_id) REFERENCES Currency(currency_id)
);

CREATE TABLE IF NOT EXISTS Inventory (
inventory_id INTEGER PRIMARY KEY AUTOINCREMENT,
player_id INTEGER NOT NULL,
item_id INTEGER NOT NULL,           
quantity INTEGER NOT NULL DEFAULT 0,
FOREIGN KEY (player_id) REFERENCES Player(player_id),
FOREIGN KEY (item_id)   REFERENCES Item(item_id)
);

CREATE TABLE IF NOT EXISTS Friend (
friend_id INTEGER PRIMARY KEY AUTOINCREMENT,
player_id INTEGER NOT NULL,
friend_player_id INTEGER NOT NULL,
status TEXT NOT NULL,
FOREIGN KEY (player_id) REFERENCES Player(player_id),
FOREIGN KEY (friend_player_id) REFERENCES Player(player_id)
);

CREATE TABLE IF NOT EXISTS Gacha_Pool (
gacha_id INTEGER PRIMARY KEY AUTOINCREMENT,
gacha_name TEXT NOT NULL,
gacha_type TEXT NOT NULL,
description TEXT
);

CREATE TABLE IF NOT EXISTS Gacha_DropRate (
drop_id INTEGER PRIMARY KEY AUTOINCREMENT,
gacha_id INTEGER NOT NULL,
item_id INTEGER NOT NULL,
base_rate REAL NOT NULL,
display_rate REAL NOT NULL,
FOREIGN KEY (gacha_id) REFERENCES Gacha_Pool(gacha_id),
FOREIGN KEY (item_id) REFERENCES Item(item_id)
);

CREATE TABLE IF NOT EXISTS Gacha_History (
history_id INTEGER PRIMARY KEY AUTOINCREMENT,
player_id INTEGER NOT NULL,
gacha_id INTEGER NOT NULL,
item_id INTEGER NOT NULL,
is_duplicate INTEGER NOT NULL DEFAULT 0,
stat_reward INTEGER,
pulled_at TEXT NOT NULL,
FOREIGN KEY (player_id) REFERENCES Player(player_id),
FOREIGN KEY (gacha_id) REFERENCES Gacha_Pool(gacha_id),
FOREIGN KEY (item_id) REFERENCES Item(item_id)
);

INSERT INTO Player (username, gold_coin, diamond) VALUES
('TiaoKai', 1200, 600),
('Maru',    1000, 600);

INSERT INTO Pet (pet_name, rarity, element, role, base_attack, base_defense, base_hp) VALUES
('dinosaur', 4, 'Fire',  'Attacker', 120, 60, 800),
('wolf', 4, 'Water', 'Support', 90, 70, 820),
('dinobats', 4, 'Earth', 'Tank', 80, 110, 1000),
('bats', 5, 'Wind', 'Attacker', 150, 100, 1200),
('sloth', 6, 'Earth', 'Support', 170, 80, 1500);

INSERT INTO Player_Pet (player_id, pet_id, level, exp, is_active) VALUES
(1, 1, 0, 0, 1);

INSERT INTO Monster (name, element, hp, attack, defense, reward_exp) VALUES
('Slime', 'Water', 120, 15,  5, 10),
('Fire Bat', 'Fire', 100, 20,  4, 12),
('Stone Crab', 'Earth', 200, 10, 15, 15);

INSERT INTO Monster_Set (monster_id, stage_order) VALUES
(1, 1),
(2, 2);

INSERT INTO Currency (currency_name) VALUES
('Gold Coin'),
('Diamond');

INSERT INTO Stage (stage_name, difficulty, reward_currency, reward_amount, monster_set_id) VALUES
('Greenfield Plains', 1, 1, 120, 1),
('Cavern of Shadows', 2, 1, 180, 2);

INSERT INTO Item (item_name, item_type, rarity, description) VALUES
('Fireflower', 'Material', 1, 'ดอกไม้ไฟ สามารถนำไปอัปเกรดสัตว์เลี้ยงได้'),
('Rabbit Foot', 'Material', 3, 'เท้ากระต่าย เพิ่มโบนัสในการอัปเกรดสัตว์เลี้ยงได้'),
('Oyster Mushroom', 'Material', 4, 'เห็ดหอยนางรม อัปเกรดสัตว์เลี้ยงเลเวล 2 ขึ้นไป'),
('dinosaur', 'PetGacha', 4, 'สัตว์เลี้ยงที่ได้จากการสุ่มกาชา'),
('wolf', 'PetGacha', 4, 'สัตว์เลี้ยงที่ได้จากการสุ่มกาชา'),
('dinobats', 'PetGacha', 4, 'สัตว์เลี้ยงที่ได้จากการสุ่มกาชา'),
('bats', 'PetGacha', 5, 'สัตว์เลี้ยงที่ได้จากการสุ่มกาชา'),
('sloth', 'PetGacha', 6, 'สัตว์เลี้ยงที่ได้จากการสุ่มกาชา');

INSERT INTO Shop (shop_name, shop_type, description) VALUES
('Pet Shop', 'Pet', 'ร้านขายไอเท็มส่วนประกอบสัตว์เลี้ยง'),
('Material Shop', 'Material', 'ร้านขายชิ้นอัปเกรดสัตว์เลี้ยง');

INSERT INTO Shop_Item (shop_id, item_id, price_currency_id, price_amount) VALUES
(2, 1, 1, 900), 
(2, 2, 1, 1200),
(2, 3, 1, 1500);

INSERT INTO Inventory (player_id, item_id, quantity) VALUES
(1, 4, 1);

INSERT INTO Friend (player_id, friend_player_id, status) VALUES
(1, 2, 'accepted');

INSERT INTO Gacha_Pool (gacha_name, gacha_type, description) VALUES
('Normal Gacha', 'Pet', 'กาชาสุ่มสัตว์เลี้ยงปกติ'),
('Season Gacha', 'Pet', 'กาชาสุ่มสัตว์เลี้ยงระดับหายาก');
	 
INSERT INTO Gacha_DropRate (gacha_id, item_id, base_rate, display_rate) VALUES
(1, 4, 0.60, 60.0),
(1, 5, 0.35, 35.0),
(1, 6, 0.20, 20.0),
(1, 7, 0.10, 10.0),
(1, 8, 0.10, 10.0);

INSERT INTO Gacha_History (player_id, gacha_id, item_id, is_duplicate, stat_reward, pulled_at) VALUES
(1, 1, 4, 0, NULL, '2025-01-12 12:00:00');


	
	