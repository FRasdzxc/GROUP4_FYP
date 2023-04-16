import random
import re
import string
from flask import Blueprint, request, abort

from poh.db import get_db

bp = Blueprint("activation", __name__)

def generate_session_token(db) -> str:
    if db is None:
        abort(500)

    characters = f"{string.ascii_letters}{string.digits}"
    while True:
        token = ''.join(random.choices(characters, k=14))
        query = db.execute("SELECT COUNT(*) as count FROM activations WHERE session_token = ?", (token,)).fetchone()
        if query['count'] == 0:
            return token

@bp.route("/activation", methods=["GET", "POST",])
def activation():
    if request.method == "GET":
        abort(404)

    db = get_db()
    fetch = db.execute("""
        SELECT
            id, session_desc, students_only
        FROM
            timeslots
        WHERE
            datetime() BETWEEN start_time AND end_time
    """).fetchone()
    if fetch is None:
        abort(403)
    session_id = fetch['id']
    session_desc = fetch['session_desc']
    validate = fetch['students_only'] == 1

    username = request.form.get("user")
    if username == None:
        abort(403)

    if validate:
        validation = re.match("^[\d]{9}$", username)
        if validation is None:
            abort(403)

    session_token = generate_session_token(db)
    db.execute("""
        INSERT INTO activations
            (session_token, session_id, username)
        VALUES
            (?, ?, ?)
    """, (
        session_token,
        session_id,
        username
    ))
    db.commit()

    return {
        'sessionToken': session_token,
        'sessionDesc': session_desc
    }
