DROP TABLE IF EXISTS timeslots;
DROP TABLE IF EXISTS activations;
DROP TABLE IF EXISTS session_records;

CREATE TABLE timeslots (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  start_time TIMESTAMP NOT NULL,
  end_time TIMESTAMP NOT NULL,
  session_desc TEXT NOT NULL,
  students_only INTEGER NOT NULL
);
INSERT INTO timeslots (start_time, end_time, session_desc, students_only)
  VALUES (datetime('2023-04-17 09:00:00', '-8 hours'), datetime('2023-04-17 16:00:00', '-8 hours'), 'Testing', 0);
INSERT INTO timeslots (start_time, end_time, session_desc, students_only)
  VALUES (datetime('2023-04-17 16:30:00', '-8 hours'), datetime('2023-04-17 18:00:00', '-8 hours'), 'CW-GSD 1A', 1);
INSERT INTO timeslots (start_time, end_time, session_desc, students_only)
  VALUES (datetime('2023-04-19 11:00:00', '-8 hours'), datetime('2023-04-19 12:30:00', '-8 hours'), 'MH-DFS 1A', 1);
INSERT INTO timeslots (start_time, end_time, session_desc, students_only)
  VALUES (datetime('2023-04-20 11:30:00', '-8 hours'), datetime('2023-04-20 12:30:00', '-8 hours'), 'CW-DFS 1C', 1);

CREATE TABLE activations (
  session_token TEXT,
  session_id INTEGER,
  username INTEGER,
  PRIMARY KEY(session_token, session_id, username),
  FOREIGN KEY(session_id) REFERENCES timeslots(id)
);

CREATE TABLE session_records (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  session_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  session_token TEXT NOT NULL,
  player_name TEXT NOT NULL,
  final_score INTEGER NOT NULL,
  hero_level INTEGER NOT NULL,
  exp_gained INTEGER NOT NULL,
  coins_earned INTEGER NOT NULL,
  coins_spent INTEGER NOT NULL,
  orb_upgrades TEXT,
  steps_taken INTEGER NOT NULL,
  damage_given REAL NOT NULL,
  damage_taken REAL NOT NULL,
  deaths INTEGER NOT NULL,
  weapon_usage TEXT,
  ability_usage TEXT,
  mob_killed TEXT,
  maps_visited TEXT,
  dungeons_cleared TEXT,
  FOREIGN KEY(session_token) REFERENCES activations(session_token)
);
