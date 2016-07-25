#Django Fundamentals
comes with ORM

loose coupling, DRY, http://goo.gl/PRrEMe

##installation
http://www.python.org/download/

http://pip-installer.org

###virtualenv
 
```
pip install virtualenv

c:\Path_To_Python\Scripts\pip install virtualenv
```

```
which python3

for %a in ("%path:;=";"%") do @echo %~a

virtualenv demo
Scripts/activate

pip install django
```

##starting a new project
```
django-admin.py startproject p1

python manage.py runserver
```

a development webserver, not to be used as production server

###django app
is a package designed to be used in a django project

```
django-admin.py startapp main

```

##views
```
//urls.py
url(r'^$', 'main.views.home'),
```
http://goo.gl/5uJsfy

`MTV` is equivalent to `MVC`

##models
inherit `models.Model`
```
from django.db import models
from django.contrib.auth.models import User

GAME_STATUS_CHOICES =(
    ('A', 'Active'),
    ('F', 'First player wins'),
    ('S', 'Second player wins'),
    ('D', 'Draw'),
)

class Move(models.Model):
    x = models.IntegerField()
    y = models.IntegerField()
    comment = models.CharField(max_length=300)

class Game(models.Model):
    first_player = models.ForeignKey(User, related_name="game_first_player")
    second_player = models.ForeignKey(User, related_name="game_second_player")
    next_to_move = models.ForeignKey(User, related_name="next_to_move")
    start_time = models.DateTimeField(auto_now_add=True)
    last_active = models.DateTimeField(auto_now=True)
    comment = models.CharField(max_length=300)
    
    def __str__(self):
        return "{0} vs {1}".format(self.first_player, self.second_player)

```

create db tables

```
python manage.py makemigrations
python manage.py migrate
#show the executed sql statement
python manage.py sqlmigrate main 0001_initial

python manage.py createsuperuser
```

south - a package for database migrations

##add models to administration

```
#admin.py
from .models import Game, Move

admin.site.register(Game)
admin.site.register(Move)
```

to view more descriptive models in the admin interface the `__str__` must be implemented

##models API
```
python manage.py shell
>>>from main.models import Game, Move
>>>Game.objects().all()
>>>m.save() #to save the object in the database

get(pk=4) #returns a single object
all()
filter(status="A")
exclude()
```

asigning objects directly works for foreign keys

##templates

```
{% if test %}
{% elif %}
{% else %}
{% endif %}
```

```
{% block content %}
 html content here
{% endblock content %}

{% extends 'base.html'  %}
```

##decorators
```
from django.contrib.auth.decorators import login_required

@login_required
def home(request):
    return render(request, 'user/home.html')
```

##templates: for, include
```
{% for item in list %}

{% empty %}

{% endfor %}
```

##csrf
```
{% csrf_token %}
```

`django-crispy-forms`

```
{% load crispy_forms_tags %}
```

##meta forms
```
class InvitationForm(ModelForm):
    class Meta:
        model = Invitation
        exclude = ['from_user']
```

##named groups in the url
```
(?P<name>expr)
```

`get_absolute_url`

##fat models, skinny views
django best practice

logic goes into models, keep views and templates simple

- DRY
- testing
- readability

##template lookup
```
{{ user.name }}
```

a `.` causes:
- dictionary lookup
- attribute lookup
- method call
- list-index lookup

```
{% block.super %}
```

```
class Meta:
    get_latest_by = 'timestamp'
```

##validators

```
y = models.IntegerField(validators=[MinValueValidator(0)])
```

for a class validation overwrite the `clean()` from Form

##generic views
###class based views
http://goo.gl/RE8ric

##debugging
```
import pdb
pdb.set_trace() 
```
```
pip install django-debug-toolbar
```

http://www.djangoproject.com

http://www.revsys.com/django/cheatsheet

http://ccbv.co.uk

http://www.djangopackages.com

http://djangosnippets.org

