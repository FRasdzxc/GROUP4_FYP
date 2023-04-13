from flask import Blueprint
from flask import render_template

from poh.db import get_db

bp = Blueprint("leaderboard", __name__)


@bp.route("/leaderboard")
def index():
    db = get_db()
    rows = db.execute("""
        SELECT
            id, player_name, final_score
        FROM
            play_sessions
        ORDER BY
            final_score DESC
        LIMIT
            10
    """).fetchall()

    leaderboard = list()
    for i in range(10):
        if i < len(rows):
            leaderboard.append((rows[i][1], rows[i][2], rows[i][0]))
        else:
            leaderboard.append(("AAA", 0, -1))
        
    print(leaderboard)
    return render_template("leaderboard/index.html", leaderboard=leaderboard)
