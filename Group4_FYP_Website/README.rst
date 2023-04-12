Path of Hero Website
====================

Install
-------

Create a virtualenv and activate it::

    $ python3 -m venv venv
    $ . venv/bin/activate

Or on Windows cmd::

    $ py -3 -m venv venv
    $ venv\Scripts\activate.bat

Install poh::

    $ pip install -e .

Or if you are using the main branch, install Flask from source before
installing poh::

    $ pip install -e ../..
    $ pip install -e .


Run
---

.. code-block:: text

    $ flask --app poh init-db
    $ flask --app poh run --debug

Open http://127.0.0.1:5000 in a browser.


Test
----

::

    $ pip install '.[test]'
    $ pytest

Run with coverage report::

    $ coverage run -m pytest
    $ coverage report
    $ coverage html  # open htmlcov/index.html in a browser
