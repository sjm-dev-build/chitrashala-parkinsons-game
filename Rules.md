# Chitrashala — Project Ruleset

This document defines the collaboration, coding, and Git rules for **Chitrashala** (Team: Madhav S, Sri Sudharshanan, S VIKRAM).

---

## 1. Git & Branching Rules
- **Never commit or push directly to `main`.** The `main` branch must always remain stable and ready for project reviews or demos.
- **Create a new branch from `main`** for every task using these naming prefixes:
  - `frontend/<task>` — HTML, CSS, Bootstrap, and JavaScript changes
  - `unity/<task>` — Unity scenes, C# scripts, and 3D/2D assets
  - `fix/<bug>` — Bug fixes in web or Unity
  - `docs/<topic>` — README, rules, reports, and documentation
- **Delete feature branches** after they are merged into `main`.

---

## 2. Commit Message Rules
Every commit message must start with a clear category prefix in lowercase:
- `feat:` — Adding a new feature or section (e.g., `feat: add fixed bootstrap navbar`)
- `fix:` — Fixing a broken layout, link, or script bug (e.g., `fix: resolve dropdown overlap`)
- `style:` — Visual/CSS spacing or color updates (e.g., `style: update team card padding`)
- `docs:` — Updating documentation or rules (e.g., `docs: add project ruleset`)

---

## 3. Frontend Rules (HTML / CSS / JS)
- Keep code clean, readable, and properly indented so every teammate can understand and explain it during viva/reviews.
- Avoid unnecessary external libraries beyond Bootstrap; keep JavaScript simple and well-commented.
- Always test `index.html` in a browser on both desktop and mobile widths before opening a Pull Request.
- Ensure all navigation links, dropdowns, and asset paths (`images/`, `css/`, `js/`) use relative paths—never local `C:\` paths.

---

## 4. Unity Collaboration Rules
- **Scene Locking Rule:** Unity `.unity` scene files and `.prefab` files cannot be auto-merged like HTML/CSS. Before editing a scene, inform the team group so two members do not modify the same scene simultaneously.
- **Keep Heavy Folders Out of Git:** Never force-add `Library/`, `Temp/`, `Obj/`, `Build/`, or `Logs/`. Always keep `.gitignore` active.
- Keep C# script names identical to their public class names (`PascalCase`, e.g., `BrushController.cs`).

---

## 5. Pull Request & Code Review Rules
- Every change must enter `main` through a **Pull Request (PR)**.
- At least **1 teammate** must review and approve the PR before merging.
- Do not merge a PR if there are unresolved merge conflicts or broken page layouts.