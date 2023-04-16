from flask import Blueprint
from flask import render_template, request

from poh.db import get_db
from poh.models import SessionRecord

bp = Blueprint("stats", __name__)


@bp.route("/stats", methods=["GET", "POST",])
def index():
    db = get_db()
    if request.method == "POST":
        record = SessionRecord.parse_form(request.form)
        record.save(db)
        return { "session_id": record.id }
    else:
        session_count, highest_level, highest_level_id, record = SessionRecord.get_accumulated(db)
        return render_template("stats/index.html", session_count=session_count, highest_level=highest_level, highest_level_id=highest_level_id, record=record)

@bp.route("/stats/<int:id>")
def detail_stats(id):
    db = get_db()
    record = SessionRecord.get_record(db, id)
    return render_template("stats/detail.html", record=record)
