# Boreal (100 Words for Snow)
A first attempt at game development in Unity. An agent-populated procedurally generated world.

# NOTES:

# Philosophy
* Change, ephemerality, non-attachment
* Sustainability, preparation
* Hope, optimism

# Aesthetic
* Simple graphics, complex world
* Orthographic / 2.5d perspective/isometric
* low-res graphics, maybe Rimworld with Another World aesthetic

# Setting
* taiga/boreal forest/tundra
* forest, lakes
* island? (helps with world limits, otherwise optional)
* (maybe) Post-collapse, the original inception was a game of preparing for a harsh winter by stockpiling resources, i.e. food, dry wood, batteries charged by pre-existing sources (i.e. solar, wind power, which may be located in remote locations), so that the player must ensure that their characters are well-prepared for the journey to collect them. These resources are then used to keep characters alive, I assume in some kind of base that can be built up. 
* Water/snow/ice is the central mechanic, as changing conditions (across seasons, day to day, day to night) make snow more or less passable, make water bodies freeze and walkable, etc.
* changing seasons, with days getting shorter, nights getting longer, weather.

# Gameplay mechanics
* Procedural generation
* Environment drives gameplay, 
  * day/night-based challenges a la survival game
  * season change from summer > winter applies pressure to prepare, increases night length vs day
  * main challenge dynamics come into play in winter, with changing conditions affecting passability, survival.
  * winter also makes new resources available, e.g. as lakes freeze and new areas become accessible (boats, but multiple unconnected (or tenuously connected) rivers and lakes requires boat to be picked up out of water and carried/dragged. Storms or some other hazard at sea could also make sea ice a big bonus for exploration)
* Oregon-trail style decisions: characters mainly act under their own AI, until a decision point is reached, where the player can make a choice. If the decision is not attended to, the character acts by themself after some time, with what may or may not be the best course of action. 
* And/Or where random/directed situations arise and a decision may or may not need to be taken. Could also be used as cover the simulations running to change world in background, and to level the difficulty, with good fortune arising when the player is doing badly, and vise versa.
* Bounded information, the player knows (only) what their characters conceivably know.
* Seasonal ramp-up in difficulty is only a function of how well a player prepares
  * Short snow-free summer season for fast exploration and collection of resources (from accessible regions, see frozen lakes, boats)
  * Nights bring challenges (may just be survival against elements, undecided)
  * Onset of winter makes survival challenging for unprepared characters, but also brings new opportunities and resources, some kind of self-levelling difficulty
  * Winter-spring transition also almost self-levelling, as increase survival time against cold is balanced against slower movement rates in slushy snow, dangerous ice.
  * Each season has its own challenges and benefits, a celebration of change
* Dynamic environment and changing terrain requires constant adaptation, no clear winning strategy.

# Pathfinding
* movement penalities imposed by terrain, particularly snow, is a key mechanic
* penalities modified by 
  * equipment, e.g. snowsnoes (skis?)
  * known/unknown terrain
  * character traits (including personal knowledge of terrain)
  * immediate visibility

## Implementation

# Balancing
* Outdoor survival time (in winter) should allow for an agent to get to resources/safe location that ensures their survival *if they are well prepared*
  1. Generate ensemble of maps with representative parameters, grouped by environmental conditions
  2. run ensemble of pathfinding routes between random pairs of points, with average movement speed, grouped by equipment/other preparation factors
  3. get statistics on speed travelled for each combination
  
  * Speed for a well-prepared character > distance between safe locations (potentially a round trip) / survival time
  * Speed for a poorly-prepared character < "
  * Balance movement penalities, bonuses, distance between safe locations and survival time accordingly
