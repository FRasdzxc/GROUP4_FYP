from datetime import datetime
import json

from flask import abort

class SessionRecord:
    id: str
    session_time: datetime
    session_token: str
    player_name: str
    final_score: int
    hero_level: int
    exp_gained: int
    coins_earned: int
    coins_spent: int
    orb_upgrades: dict
    steps_taken: int
    damage_given: float
    damage_taken: float
    deaths: int
    weapon_usage: dict
    ability_usage: dict
    mob_killed: dict
    maps_visited: list
    dungeons_cleared: dict

    def __init__(self):
        self.id = None
        self.session_time = None
        self.session_token = None
        self.player_name = None
        self.final_score = 0
        self.hero_level = 0
        self.exp_gained = 0
        self.coins_earned = 0
        self.coins_spent = 0
        self.orb_upgrades = None
        self.steps_taken = 0
        self.damage_given = 0
        self.damage_taken = 0
        self.deaths = 0
        self.weapon_usage = None
        self.ability_usage = None
        self.mob_killed = None
        self.maps_visited = None
        self.dungeons_cleared = None

    def count_orb_upgrades(self, upgrade_type=None):
        if getattr(self, 'orb_upgrades', None) == None:
            return 0
        
        if upgrade_type != None:
            return self.orb_upgrades[upgrade_type] if upgrade_type in self.orb_upgrades else 0
        else:
            return sum(x for x in self.orb_upgrades.values()) if getattr(self, 'orb_upgrades', None) != None else 0
    def count_weapon_usage(self, *ids):
        if getattr(self, 'weapon_usage', None) == None:
            return 0
        
        if len(ids) > 1:
            return sum(self.count_weapon_usage(w) for w in ids)
        elif len(ids) == 1:
            return self.weapon_usage[ids[0]] if ids[0] in self.weapon_usage else 0
        else:
            return sum(x for x in self.weapon_usage.values()) if getattr(self, 'weapon_usage', None) != None else 0
    def count_ability_usage(self, ability_id=None):
        if getattr(self, 'ability_usage', None) == None:
            return 0
        
        if ability_id != None:
            return self.ability_usage[ability_id] if ability_id in self.ability_usage else 0
        else:
            return sum(x for x in self.ability_usage.values()) if getattr(self, 'ability_usage', None) != None else 0
    def count_mob_killed(self, mod_id=None):
        if getattr(self, 'mob_killed', None) == None:
            return 0
        
        if mod_id != None:
            return self.mob_killed[mod_id] if mod_id in self.mob_killed else 0
        else:
            return sum(x for x in self.mob_killed.values()) if getattr(self, 'mob_killed', None) != None else 0
    def count_maps_visited(self):
        return len(self.maps_visited) if getattr(self, 'maps_visited', None) != None else 0
    def count_dungeons_cleared(self, dungeon_id=None):
        if getattr(self, 'dungeons_cleared', None) == None:
            return 0
        
        if dungeon_id != None:
            return self.dungeons_cleared[dungeon_id] if dungeon_id in self.dungeons_cleared else 0
        else:
            return sum(x for x in self.dungeons_cleared.values()) if getattr(self, 'dungeons_cleared', None) != None else 0
    def save(self, db):
        if getattr(self, 'id', None) is not None:
            return

        db.execute("""
            INSERT INTO session_records (
                session_token,
                player_name,
                final_score,
                hero_level,
                exp_gained,
                coins_earned,
                coins_spent,
                orb_upgrades,
                steps_taken,
                damage_given,
                damage_taken,
                deaths,
                weapon_usage,
                ability_usage,
                mob_killed,
                maps_visited,
                dungeons_cleared
            ) VALUES
                (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """, (
            self.session_token,
            self.player_name,
            self.final_score,
            self.hero_level,
            self.exp_gained,
            self.coins_earned,
            self.coins_spent,
            json.dumps(self.orb_upgrades) if getattr(self, 'orb_upgrades', None) != None else None,
            self.steps_taken,
            self.damage_given,
            self.damage_taken,
            self.deaths,
            json.dumps(self.weapon_usage) if getattr(self, 'weapon_usage', None) != None else None,
            json.dumps(self.ability_usage) if getattr(self, 'ability_usage', None) != None else None,
            json.dumps(self.mob_killed) if getattr(self, 'mob_killed', None) != None else None,
            json.dumps(self.maps_visited) if getattr(self, 'maps_visited', None) != None else None,
            json.dumps(self.dungeons_cleared) if getattr(self, 'dungeons_cleared', None) != None else None,
        ))
        db.commit()
        fetch = db.execute("SELECT id, session_time FROM session_records WHERE session_token = ?", (self.session_token,)).fetchone()
        self.id = fetch["id"]
        self.session_time = fetch["session_time"]
        return self.id

    @staticmethod
    def get_quick_stats(db):
        result = db.execute("""
            SELECT 
                COUNT(id) as session_played,
                SUM(steps_taken) as steps_taken
            FROM
                session_records
        """).fetchone()

        mobs_killed = 0
        rows = db.execute("SELECT mob_killed FROM session_records WHERE mob_killed IS NOT NULL").fetchall()
        for row in rows:
            mobs_killed += sum(x for x in json.loads(row['mob_killed']).values())

        return {
            'sessions_played': result['session_played'],
            'steps_taken': result['steps_taken'],
            'mobs_killed': mobs_killed
        }
    @staticmethod
    def update_sum(a: dict, b:dict) -> dict:
        if a is None or b is None:
            abort(500)

        return {
            key: a.get(key, 0) + b.get(key, 0)
            for key in set(a) | set(b)
        }
    @staticmethod
    def get_accumulated(db):
        higest_level = 0
        higest_level_id = -1
        session_count = 0
        result = SessionRecord()

        id_rows = db.execute("SELECT id FROM session_records").fetchall()
        for id_row in id_rows:
            record = SessionRecord.get_record(db, id_row["id"])
            if record is None:
                continue

            session_count += 1

            if record.hero_level > higest_level:
                higest_level = record.hero_level
                higest_level_id = record.id

            result.exp_gained += record.exp_gained
            result.coins_earned += record.coins_earned
            result.coins_spent += record.coins_spent

            if record.orb_upgrades is not None:
                if result.orb_upgrades is None:
                    result.orb_upgrades = record.orb_upgrades
                else:
                    result.orb_upgrades = SessionRecord.update_sum(result.orb_upgrades, record.orb_upgrades)

            result.steps_taken += record.steps_taken
            result.damage_given += record.damage_given
            result.damage_taken += record.damage_taken
            result.deaths += record.deaths

            if record.weapon_usage is not None:
                if result.weapon_usage is None:
                    result.weapon_usage = record.weapon_usage
                else:
                    result.weapon_usage = SessionRecord.update_sum(result.weapon_usage, record.weapon_usage)

            if record.ability_usage is not None:
                if result.ability_usage is None:
                    result.ability_usage = record.ability_usage
                else:
                    result.ability_usage = SessionRecord.update_sum(result.ability_usage, record.ability_usage)

            if record.mob_killed is not None:
                if result.mob_killed is None:
                    result.mob_killed = record.mob_killed
                else:
                    result.mob_killed = SessionRecord.update_sum(result.mob_killed, record.mob_killed)

            if record.maps_visited is not None:
                if result.maps_visited is None:
                    result.maps_visited = record.maps_visited
                else:
                    result.maps_visited += record.maps_visited

            if record.dungeons_cleared is not None:
                if result.dungeons_cleared is None:
                    result.dungeons_cleared = record.dungeons_cleared
                else:
                    result.dungeons_cleared = SessionRecord.update_sum(result.dungeons_cleared, record.dungeons_cleared)

        return (session_count, higest_level, higest_level_id, result)
    @staticmethod
    def get_leaderboard(db, timeslot_id=None):
        ranking = ('1st', '2nd', '3rd', '4th', '5th', '6th', '7th', '8th', '9th')
        rows = None
        if timeslot_id is not None:
            rows = db.execute("""
                SELECT
                    s.id as id,
                    s.player_name as player_name,
                    s.final_score as final_score
                FROM session_records s
                INNER JOIN activations a ON a.session_token = s.session_token
                INNER JOIN timeslots t ON a.session_id = t.id
                WHERE t.id = ?
                ORDER BY final_score DESC
                LIMIT ?
            """, (timeslot_id, len(ranking),)).fetchall()
        else:
            rows = db.execute("""
                SELECT
                    id,
                    player_name,
                    final_score
                FROM session_records
                ORDER BY final_score DESC
                LIMIT ?
            """, (len(ranking),)).fetchall()

        if rows is None:
            abort(404)

        result = list()
        for i, rank in enumerate(ranking):
            if i >= len(rows):
                result.append((rank, None))
            else:
                result.append((rank, {
                    'id': rows[i][0],
                    'player_name': rows[i][1],
                    'final_score': rows[i][2],
                }))
        return result
    @staticmethod
    def get_record(db, id):
        fetch = db.execute("""
            SELECT
                id,
                session_time,
                session_token,
                player_name,
                final_score,
                hero_level,
                exp_gained,
                coins_earned,
                coins_spent,
                orb_upgrades,
                steps_taken,
                damage_given,
                damage_taken,
                deaths,
                weapon_usage,
                ability_usage,
                mob_killed,
                maps_visited,
                dungeons_cleared
            FROM session_records WHERE id = ?
            """, (id,)).fetchone()
        if fetch is None:
            abort(404)

        return SessionRecord.parse_db(fetch)

    @staticmethod
    def parse_db(fetch):
        result = SessionRecord()

        result.id = fetch["id"]
        result.session_time = fetch["session_time"]
        result.session_token = fetch["session_token"]
        result.player_name = fetch["player_name"]
        result.final_score = fetch["final_score"]
        result.hero_level = fetch["hero_level"]
        result.exp_gained = fetch["exp_gained"]
        result.coins_earned = fetch["coins_earned"]
        result.coins_spent = fetch["coins_spent"]
        orb_upgrades = fetch["orb_upgrades"]
        if orb_upgrades is not None:
            result.orb_upgrades = json.loads(orb_upgrades)
        result.steps_taken = fetch["steps_taken"]
        result.damage_given = fetch["damage_given"]
        result.damage_taken = fetch["damage_taken"]
        result.deaths = fetch["deaths"]
        weapon_usage = fetch["weapon_usage"]
        if weapon_usage is not None:
            result.weapon_usage = json.loads(weapon_usage)
        ability_usage = fetch["ability_usage"]
        if ability_usage is not None:
            result.ability_usage = json.loads(ability_usage)
        mob_killed = fetch["mob_killed"]
        if mob_killed is not None:
            result.mob_killed = json.loads(mob_killed)
        maps_visited = fetch["maps_visited"]
        if maps_visited is not None:
            result.maps_visited = json.loads(maps_visited)
        dungeons_cleared = fetch["dungeons_cleared"]
        if dungeons_cleared is not None:
            result.dungeons_cleared = json.loads(dungeons_cleared)

        return result
    @staticmethod
    def parse_form(form):
        result = SessionRecord()

        session_token = form.get("sessionToken")
        if session_token is not None:
            result.session_token = session_token
        else:
            abort(400)

        player_name = form.get("playerName")
        if player_name is not None:
            result.player_name = player_name
        else:
            abort(400)

        final_score = form.get("finalScore", type=int)
        if final_score is not None:
            result.final_score = final_score
        else:
            abort(400)

        hero_level = form.get("heroLevel", type=int)
        if hero_level is not None:
            result.hero_level = hero_level
        else:
            abort(400)

        exp_gained = form.get("expGained", type=int)
        if exp_gained is not None:
            result.exp_gained = exp_gained
        else:
            abort(400)

        coins_earned = form.get("coinsEarned", type=int)
        if coins_earned is not None:
            result.coins_earned = coins_earned
        else:
            abort(400)

        coins_spent = form.get("coinsSpent", type=int)
        if coins_spent is not None:
            result.coins_spent = coins_spent
        else:
            abort(400)

        orb_upgrades = form.get("orbUpgrades")
        if orb_upgrades is not None:
            result.orb_upgrades = json.loads(orb_upgrades)

        steps_taken = form.get("stepsTaken", type=int)
        if steps_taken is not None:
            result.steps_taken = steps_taken
        else:
            abort(400)

        damage_given = form.get("damageGiven", type=float)
        if damage_given is not None:
            result.damage_given = damage_given
        else:
            abort(400)

        damage_taken = form.get("damageTaken", type=float)
        if damage_taken is not None:
            result.damage_taken = damage_taken
        else:
            abort(400)

        deaths = form.get("deaths", type=int)
        if deaths is not None:
            result.deaths = deaths
        else:
            abort(400)

        weapon_usage = form.get("weaponUsage")
        if weapon_usage is not None:
            result.weapon_usage = json.loads(weapon_usage)

        ability_usage = form.get("abilityUsage")
        if ability_usage is not None:
            result.ability_usage = json.loads(ability_usage)

        mob_killed = form.get("mobKilled")
        if mob_killed is not None:
            result.mob_killed = json.loads(mob_killed)

        maps_visited = form.get("mapsVisited")
        if maps_visited is not None:
            result.maps_visited = json.loads(maps_visited)

        dungeons_cleared = form.get("dungeonsCleared")
        if dungeons_cleared is not None:
            result.dungeons_cleared = json.loads(dungeons_cleared)

        return result
