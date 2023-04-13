import json
from flask import Blueprint
from flask import render_template, abort, request, redirect, url_for

from poh.db import get_db

bp = Blueprint("stats", __name__)


@bp.route("/stats")
def index():
    db = get_db()
    accumulated = db.execute("""
        SELECT
            COUNT(id) as session_count,
            SUM(steps_taken) as steps_taken,
            SUM(damage_given) as damage_given,
            SUM(damage_taken) as damage_taken,
            SUM(weapon_usage) as weapon_attacks,
            SUM(ability_usage) as ability_attacks,
            SUM(mobs_killed) as mobs_killed,
            SUM(dungeons_cleared) as dungeons_cleared
        FROM
            play_sessions
    """).fetchone()

    weapon = {
        'wand': 0,
        'whirlwind': 0
    }
    rows = db.execute("SELECT weapon_usage_detail FROM play_sessions").fetchall()
    for row in rows:
        detail_json = json.loads(row[0])
        wands = ('weapon_wandtier0', 'weapon_wandtier1', 'weapon_wandtier2', 'weapon_wandtier3')
        for wand in wands:
            if wand in detail_json:
                weapon['wand'] += int(detail_json[wand])
        whirlwinds = ('weapon_whirlwindtier0', 'weapon_whirlwindtier1', 'weapon_whirlwindtier2', 'weapon_whirlwindtier3')
        for whirlwind in whirlwinds:
            if whirlwind in detail_json:
                weapon['whirlwind'] += int(detail_json[whirlwind])

    ability = {
        'fireball': 0,
        'soul_ring': 0,
        'tornado': 0
    }
    rows = db.execute("SELECT ability_usage_detail FROM play_sessions").fetchall()
    for row in rows:
        detail_json = json.loads(row[0])
        if 'FireballV2' in detail_json:
            ability['fireball'] += int(detail_json['FireballV2'])
        if 'SoulRing' in detail_json:
            ability['soul_ring'] += int(detail_json['SoulRing'])
        if 'Tornado' in detail_json:
            ability['tornado'] += int(detail_json['Tornado'])
    
    mobs = {
        'slime': 0,
    }
    rows = db.execute("SELECT mob_killed_detail FROM play_sessions").fetchall()
    for row in rows:
        detail_json = json.loads(row[0])
        if 'Slime' in detail_json:
            mobs['slime'] += int(detail_json['Slime'])

    return render_template("stats/index.html", accumulated=accumulated, weapon=weapon, ability=ability, mobs=mobs)

def get_session(id):
    play_session = get_db().execute("SELECT * FROM play_sessions WHERE id = ?", (id,)).fetchone()
    if play_session is None:
        abort(404, f"Play session id {id} doesn't exist.")

    weapon = {
        'wand': 0,
        'whirlwind': 0
    }
    detail_json = json.loads(play_session["weapon_usage_detail"])
    wands = ('weapon_wandtier0', 'weapon_wandtier1', 'weapon_wandtier2', 'weapon_wandtier3')
    for wand in wands:
        if wand in detail_json:
            weapon['wand'] += int(detail_json[wand])
    whirlwinds = ('weapon_whirlwindtier0', 'weapon_whirlwindtier1', 'weapon_whirlwindtier2', 'weapon_whirlwindtier3')
    for whirlwind in whirlwinds:
        if whirlwind in detail_json:
            weapon['whirlwind'] += int(detail_json[whirlwind])

    ability = {
        'fireball': 0,
        'soul_ring': 0,
        'tornado': 0
    }
    detail_json = json.loads(play_session["ability_usage_detail"])
    if 'FireballV2' in detail_json:
        ability['fireball'] += int(detail_json['FireballV2'])
    if 'SoulRing' in detail_json:
        ability['soul_ring'] += int(detail_json['SoulRing'])
    if 'Tornado' in detail_json:
        ability['tornado'] += int(detail_json['Tornado'])
    
    mobs = {
        'slime': 0,
    }
    detail_json = json.loads(play_session["mob_killed_detail"])
    if 'Slime' in detail_json:
        mobs['slime'] += int(detail_json['Slime'])

    obj = {
        "player_name": play_session["player_name"],
        "final_score": play_session["final_score"],
        "steps_taken": play_session["steps_taken"],
        "damage_given": play_session["damage_given"],
        "damage_taken": play_session["damage_taken"],
        "weapon_usage": play_session["weapon_usage"],
        "ability_usage": play_session["ability_usage"],
        "mobs_killed": play_session["mobs_killed"],
        "dungeons_cleared": play_session["dungeons_cleared"],
        "weapon_usage_detail": weapon,
        "ability_usage_detail": ability,
        "mob_killed_detail": mobs,
        "maps_visited": json.loads(play_session["maps_visited"]),
        "dungeons_cleared_detail": json.loads(play_session["dungeons_cleared_detail"])
    }
    return obj

@bp.route("/stats/<int:id>")
def detail_stats(id):
    play_session = get_session(id)
    return render_template("stats/detail.html", play_session=play_session)

@bp.route("/upload_stats", methods=["POST",])
def upload_stats():
    print()
    db = get_db()
    db.execute("""
        INSERT INTO play_sessions
            (player_name,
            final_score,
            steps_taken,
            damage_given,
            damage_taken,
            weapon_usage,
            ability_usage,
            mobs_killed,
            dungeons_cleared,
            weapon_usage_detail,
            ability_usage_detail,
            mob_killed_detail,
            maps_visited,
            dungeons_cleared_detail)
        VALUES
            (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
    """, (
        request.form["playerName"],
        int(request.form["finalScore"]),
        request.form["stepsTaken"],
        float(request.form["damageGiven"]),
        float(request.form["damageTaken"]),
        request.form["weaponUsage"],
        request.form["abilityUsage"],
        request.form["mobKilled"],
        request.form["dungeonsCleared"],
        request.form["weaponUsageDetail"],
        request.form["abilityUsageDetail"],
        request.form["mobKilledDetail"],
        request.form["mapsVisited"],
        request.form["dungeonsClearedDetail"]
    ))
    db.commit()
    return redirect(url_for("stats.index"))
