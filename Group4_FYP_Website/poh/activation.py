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
            id, session_desc
        FROM
            timeslots
        WHERE
            datetime() BETWEEN start_time AND end_time
    """).fetchone()
    if fetch is None:
        abort(403)
    session_id = fetch['id']
    session_desc = fetch['session_desc']

    user = request.form.get("user")
    if user == None:
        abort(403)

    validation = re.search("([A-Za-z0-9 ]+) \(([\d]{9})\)", user)
    if validation is None:
        abort(403)

    student_id = validation.group(2)
    student_name = validation.group(1)

    session_token = None
    fetch = db.execute("""
        SELECT session_token
        FROM activations
        WHERE
            session_id = ? AND student_id = ? AND student_name = ?
    """, (session_id, student_id, student_name,)).fetchone()
    if fetch is None:
        session_token = generate_session_token(db)
        db.execute("""
            INSERT INTO activations
                (session_token, session_id, student_id, student_name)
            VALUES
                (?, ?, ?, ?)
        """, (
            session_token,
            session_id,
            student_id,
            student_name
        ))
        db.commit()
    else:
        session_token = fetch["session_token"]

    return {
        'sessionToken': session_token,
        'sessionDesc': session_desc
    }
