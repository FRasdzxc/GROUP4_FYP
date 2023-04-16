from datetime import timezone

from flask import Blueprint
from flask import render_template

from poh.db import get_db
from poh.models import SessionRecord

bp = Blueprint("leaderboard", __name__)


@bp.route("/leaderboard")
def index():
    db = get_db()
    leaderboard = SessionRecord.get_leaderboard(db)
    return render_template("leaderboard/index.html", leaderboard=leaderboard)

@bp.route("/leaderboard/<int:id>")
def by_timeslot(id):
    db = get_db()
    leaderboard = SessionRecord.get_leaderboard(db, id)
    row = db.execute("SELECT start_time, end_time, session_desc FROM timeslots WHERE id = ?", (id,)).fetchone()
    session_info = {
        'start_time': row["start_time"].replace(tzinfo=timezone.utc).astimezone(tz=None).strftime("%d-%m-%Y %I:%M %p"),
        'end_time': row["end_time"].replace(tzinfo=timezone.utc).astimezone(tz=None).strftime("%d-%m-%Y %I:%M %p"),
        'session_desc': row["session_desc"],
    }
    return render_template("leaderboard/index.html", session_info=session_info, leaderboard=leaderboard)
