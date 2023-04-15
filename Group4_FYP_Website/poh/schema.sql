DROP TABLE IF EXISTS timeslots;
DROP TABLE IF EXISTS activations;
DROP TABLE IF EXISTS session_records;

CREATE TABLE timeslots (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  start_time TIMESTAMP NOT NULL,
  end_time TIMESTAMP NOT NULL,
  session_desc TEXT NOT NULL
);
INSERT INTO timeslots (start_time, end_time, session_desc) VALUES (datetime(), datetime('now', '+2 hours'), 'Development');
INSERT INTO timeslots (start_time, end_time, session_desc) VALUES (datetime('2023-04-17 16:30:00', '-8 hours'), datetime('2023-04-17 18:00:00', '-8 hours'), 'CW-GSD 1A');
INSERT INTO timeslots (start_time, end_time, session_desc) VALUES (datetime('2023-04-19 11:00:00', '-8 hours'), datetime('2023-04-19 12:30:00', '-8 hours'), 'MH-DFS 1A');
INSERT INTO timeslots (start_time, end_time, session_desc) VALUES (datetime('2023-04-20 11:30:00', '-8 hours'), datetime('2023-04-20 12:30:00', '-8 hours'), 'CW-DFS 1C');

CREATE TABLE activations (
  session_token TEXT,
  session_id INTEGER,
  student_id INTEGER,
  student_name TEXT NOT NULL,
  PRIMARY KEY(session_token, session_id, student_id),
  FOREIGN KEY(session_id) REFERENCES timeslots(id)
);

CREATE TABLE session_records (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  upload_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  session_token TEXT NOT NULL,
  player_name TEXT NOT NULL,
  final_score INTEGER NOT NULL,
  steps_taken INTEGER NOT NULL,
  damage_given REAL NOT NULL,
  damage_taken REAL NOT NULL,
  weapon_usage INTEGER NOT NULL,
  ability_usage INTEGER NOT NULL,
  mobs_killed INTEGER NOT NULL,
  dungeons_cleared INTEGER NOT NULL,
  weapon_usage_detail TEXT,
  ability_usage_detail TEXT,
  mob_killed_detail TEXT,
  maps_visited TEXT,
  dungeons_cleared_detail TEXT,
  FOREIGN KEY(session_token) REFERENCES activations(session_token)
);
