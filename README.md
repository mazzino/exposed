# Exposed - rule-based references

**Have you ever faced such problems:**
• **Recreating references** for your components over and over again?
• **No control** how the references are actually configured?
• **No control over** the references **in the inspector** when setting them in code?
• End up frustrated with the **slow scene loading** when setting the references in code?

**Well, here comes the solution.**

EXPOSED is a simple but powerful tool which helps you easily and automatically **map object references** for your components. You can **set rules** for creating references. Plus these references are completely **reusable** for other components. Everything is **configurable in the Inspector** and you can **see the results** of rules settings and references immediately.

**EXPOSED vs. drag&drop**
Comparing to ordinary drag&dropping EXPOSED dramatically reduces repeatable d&d or reference selecting.

**EXPOSED vs. coding**
EXPOSED helps you to visualize the reference settings and to get rid of initialization performance overhead when setting references by code in Awake method.

**How to use it**
• Add ExposedReferences script on GameObject with your component.
• Create reusable exposed configuration asset file.
• Choose which references will be assigned automatically through EXPOSED and which will be handled manually.
• Define the rules.

**Rules’ settings**
There’s no need for coding when setting the rules, you can always use basic predefined rules. However, if you need to define extra conditions for obtaining references for scene objects, you can write your own rules and connect them through EXPOSED API. Or you will be able to use the query, which is realtime and the most flexible (query feature coming soon).

**Predefined rules**
• GameObject – searching for components on the same GameObject
• Children – searching for components or GameObjects in the children GameObjects
• Parents – searching for components or GameObjects in the parents GameObjects
• Tag – searching for components or GameObjects with specified tag in scene
• Object of Type – searching for objects of specified type in scene
