import os

from flask import Flask, render_template


def create_app():
    """Create and configure an instance of the Flask application."""
    app = Flask(__name__, instance_relative_config=True)
    app.config.from_mapping(
        SECRET_KEY="dev",
        DATABASE=os.path.join(app.instance_path, "poh.sqlite"),
    )
    app.config.from_pyfile("config.py", silent=True)

    try:
        os.makedirs(app.instance_path)
    except OSError:
        pass

    from poh import db
    db.init_app(app)

    @app.errorhandler(404)
    def page_not_found(error):
        return render_template("error/page_not_found.html"), 404

    from poh import activation, leaderboard, stats
    app.register_blueprint(activation.bp)
    app.register_blueprint(leaderboard.bp)
    app.register_blueprint(stats.bp)

    @app.route("/")
    def index():
        quick_stats = db.get_db().execute("""
            SELECT 
                COUNT(id) as session_played,
                SUM(steps_taken) as steps_taken,
                SUM(mobs_killed) as mobs_killed
            FROM
                session_records
        """).fetchone()
        return render_template('index.html', quick_stats=quick_stats)

    return app
