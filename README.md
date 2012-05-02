Internals
=========

This project is all about internals. You know, those things you find yourself cutting and pasting between project, yet never quite keeping each version updated. Sure, it's great to be able to pull code from someplace that solves a problem, and once it works, really, there is no reason to update it unless something new is needed or a bug is found.

Ah yes, but a bug will be found. And you'll fix it in one project, but neglect to fix it in another. And that can be painful.

So what do you do? You create a KitchenSink project. You now have a cool assembly that is separate and contains all of your common bits of code. Now you have two problems. Bugs and keeping all your projects on the latest depedencies. The solution? Well, let's use ilmerge and make it all one assembly. Now you that THREE problems.

Let's recap. That previous paragraph is a world of pain and misery.

Solution?
---------

Create a GitHub repository, call it Internals, and put all your common code in there. Now, put it all in a non-specific namespace like, well, _Internals_.

Now you can add this repository as a submodule to your other projects. The code is in one place, it's all internal (nothing in the Internals repository is marked public, _nothing!_). And the world is a happier place.

Is that it?
-----------

Yes, that's it. Feel free to reference it in your project, feel free to clone and submit pull requests.

Enjoy!

Chris Patterson
@phatboyg
