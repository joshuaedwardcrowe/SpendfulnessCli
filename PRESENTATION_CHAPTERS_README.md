# DotNetNorth Presentation Files

This directory contains the presentation "From Hello World to a Scalable CLI Architecture" split into individual chapter files for easy editing and presentation.

## File Structure

The presentation is split into the following files:

### Main Files

- **PRESENTATION.md** - Complete presentation in one file (for reference)
- **PRESENTATION_00_INTRO.md** - Title, metadata, and table of contents
- **PRESENTATION_01_HOOK.md** - Section 1: Hook - Live Demo (2 min)
- **PRESENTATION_02_WHO_AM_I.md** - Section 2: Who Am I (3 min)
- **PRESENTATION_03_WHAT_IS_TALK_ABOUT.md** - Section 3: What Is This Talk About (5 min)
- **PRESENTATION_04_WHERE_DID_I_CHEAT.md** - Section 4: Where Did I Cheat (5 min)
- **PRESENTATION_05_LIVE_DEMO.md** - Section 5: LIVE DEMO - Hello World to Reusable (10 min)
- **PRESENTATION_06_HOW_IT_WORKS.md** - Section 6: HOW IT WORKS (15 min)
- **PRESENTATION_07_WHERE_NEXT.md** - Section 7: WHERE NEXT (3 min)
- **PRESENTATION_08_QA.md** - Section 8: Q&A (2 min)
- **PRESENTATION_09_CLOSING.md** - Summary, Resources, and Thank You

### Supporting Files

- **PRESENTATION_NOTES.md** - Presenter notes and tips
- **PRESENTATION_README.md** - General presentation usage instructions
- **PRESENTATION_SUMMARY.txt** - Quick reference statistics

## Usage

### Editing Individual Sections

Each section is in its own file, making it easy to:
- Edit specific sections without affecting others
- Review changes to individual sections
- Collaborate with different people on different sections
- Present from individual files if needed

### Presenting from Individual Files

You can present from individual files by opening them sequentially, or use a presentation tool that supports multiple markdown files.

#### Option 1: Markdown Viewer
Use a markdown viewer that can navigate between files:
- VS Code with Markdown Preview Enhanced
- Obsidian in presentation mode
- Marked 2 (macOS)

#### Option 2: Combine Files for Presentation
If you need a single file for your presentation tool:

```bash
# Combine all files into one
cat PRESENTATION_00_INTRO.md \
    PRESENTATION_01_HOOK.md \
    PRESENTATION_02_WHO_AM_I.md \
    PRESENTATION_03_WHAT_IS_TALK_ABOUT.md \
    PRESENTATION_04_WHERE_DID_I_CHEAT.md \
    PRESENTATION_05_LIVE_DEMO.md \
    PRESENTATION_06_HOW_IT_WORKS.md \
    PRESENTATION_07_WHERE_NEXT.md \
    PRESENTATION_08_QA.md \
    PRESENTATION_09_CLOSING.md \
    > PRESENTATION_COMBINED.md
```

#### Option 3: Convert to Slides
Use Marp or reveal-md to convert individual sections:

```bash
# Convert each section to slides
for file in PRESENTATION_*.md; do
    marp "$file" -o "${file%.md}.html"
done
```

## Section Overview

| File | Section | Duration | Content |
|------|---------|----------|---------|
| 00 | Intro | - | Title, metadata, table of contents |
| 01 | Hook | 2 min | Simple hello world CLI demo |
| 02 | Who Am I | 3 min | BrightHR context and motivation |
| 03 | What Is This Talk About | 5 min | Throwaway scripts problem, CLI solution |
| 04 | Where Did I Cheat | 5 min | Dependencies: MediatR, DI, ConsoleTables, CsvHelper |
| 05 | Live Demo | 10 min | Hello world → Reusable → Composition |
| 06 | How It Works | 15 min | 7 architecture components deep dive |
| 07 | Where Next | 3 min | Future roadmap and experiments |
| 08 | Q&A | 2 min | Common questions |
| 09 | Closing | - | Summary, resources, thank you |

**Total Duration:** 45 minutes

## Making Changes

### Edit a Single Section
1. Open the relevant `PRESENTATION_XX_*.md` file
2. Make your changes
3. Save the file
4. The change is isolated to that section

### Update the Combined Presentation
If you've made changes to individual files and need to update the main `PRESENTATION.md`:

```bash
# Regenerate the combined presentation
cat PRESENTATION_00_INTRO.md \
    PRESENTATION_01_HOOK.md \
    PRESENTATION_02_WHO_AM_I.md \
    PRESENTATION_03_WHAT_IS_TALK_ABOUT.md \
    PRESENTATION_04_WHERE_DID_I_CHEAT.md \
    PRESENTATION_05_LIVE_DEMO.md \
    PRESENTATION_06_HOW_IT_WORKS.md \
    PRESENTATION_07_WHERE_NEXT.md \
    PRESENTATION_08_QA.md \
    PRESENTATION_09_CLOSING.md \
    > PRESENTATION.md
```

### Review Changes
Use git diff to see changes to individual sections:

```bash
git diff PRESENTATION_05_LIVE_DEMO.md
```

## Tips

### For Presenters
- Use `PRESENTATION_NOTES.md` for additional context and tips
- Practice timing with each individual section
- Keep `PRESENTATION_00_INTRO.md` open for quick reference to structure

### For Collaborators
- Each person can work on different sections without merge conflicts
- Use section files for focused code reviews
- Easy to swap out or reorder sections

### For Version Control
- Changes to individual sections create cleaner git history
- Easier to see what changed in each section
- Can revert changes to specific sections without affecting others

## Presentation Tools

### Recommended Tools for Split Files

1. **VS Code with Extensions**
   - Markdown Preview Enhanced
   - Marp for VS Code
   - Can navigate between files easily

2. **Obsidian**
   - Great markdown support
   - Can link between files
   - Presentation mode available

3. **reveal-md**
   - Supports multiple markdown files
   - Can include files in main presentation
   - Live reload during development

4. **Deckset (macOS)**
   - Can import multiple markdown files
   - Professional presentation mode

## Converting Back to Single File

If you need to go back to a single file:

```bash
# Create combined file
cat PRESENTATION_0*.md > PRESENTATION_FULL.md
```

Or use the existing `PRESENTATION.md` which contains the full presentation.

## Questions?

See `PRESENTATION_README.md` for general presentation usage instructions, or check the main repository README.
