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

    from poh.models import SessionRecord

    @app.route("/")
    def index():
        quick_stats = SessionRecord.get_quick_stats(db.get_db())
        return render_template('index.html', quick_stats=quick_stats)

    return app
