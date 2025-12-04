# XMI Schema C# Documentation

This folder contains the static documentation site for the XMI Schema C# library, built with Jekyll and hosted on GitHub Pages.

## Structure

```
docs/
├── _config.yml              # Jekyll configuration
├── index.md                 # Homepage
├── api/                     # API reference documentation
│   ├── index.md            # API overview
│   ├── base-types.md       # Base classes
│   ├── entities.md         # Entity overview
│   ├── physical.md         # Physical elements
│   ├── structural-analytical.md  # Analytical elements
│   ├── relationships.md    # Relationships
│   ├── enums.md           # Enumerations
│   └── geometries.md      # Geometry primitives
├── reference/              # Reference guides
│   └── shape-parameters.md  # Cross-section parameters
└── examples/               # Usage examples
    └── usage.md           # Common scenarios
```

## Local Development

To preview the site locally:

1. Install Ruby and Bundler (if not already installed)
2. Install Jekyll:
   ```bash
   gem install jekyll bundler
   ```
3. Navigate to the docs folder:
   ```bash
   cd docs
   ```
4. Create a Gemfile (if needed):
   ```ruby
   source "https://rubygems.org"
   gem "jekyll", "~> 4.3"
   gem "jekyll-theme-cayman"
   ```
5. Install dependencies:
   ```bash
   bundle install
   ```
6. Serve the site:
   ```bash
   bundle exec jekyll serve
   ```
7. Open http://localhost:4000/xmi-schema-csharp/ in your browser

## GitHub Pages Setup

### Enable GitHub Pages

1. Go to your repository on GitHub
2. Navigate to **Settings** → **Pages**
3. Under **Source**, select:
   - Branch: `main`
   - Folder: `/docs`
4. Click **Save**
5. GitHub will automatically build and deploy the site
6. Your site will be available at: `https://xmi-schema.github.io/xmi-schema-csharp/`

### Automatic Deployment

GitHub Pages automatically rebuilds the site when you:
- Push changes to the `main` branch
- Merge pull requests
- Edit files directly on GitHub

No manual build or deployment steps are required!

## Updating Documentation

### Adding a New Page

1. Create a new `.md` file in the appropriate folder
2. Add front matter at the top:
   ```yaml
   ---
   layout: default
   title: Page Title
   ---
   ```
3. Write your content in Markdown
4. Link to it from other pages or the navigation

### Editing Existing Pages

Simply edit the `.md` files and push to GitHub. Changes will be live within a few minutes.

### Adding Code Examples

Use fenced code blocks with language specification:

````markdown
```csharp
var beam = new XmiBeam(...);
```
````

### Adding Tables

Use standard Markdown table syntax:

```markdown
| Column 1 | Column 2 |
| --- | --- |
| Value 1 | Value 2 |
```

## Theme Customization

The site uses the `jekyll-theme-cayman` theme. To customize:

1. Override theme defaults in `_config.yml`
2. Add custom CSS in `assets/css/style.scss`
3. Override layouts by creating files in `_layouts/`

## Best Practices

- Keep documentation in sync with code
- Use clear, concise language
- Provide code examples for all major features
- Link between related pages
- Update version numbers when releasing
- Test links before publishing

## Troubleshooting

### Site not building?

- Check the **Actions** tab on GitHub for build errors
- Verify YAML front matter is valid
- Ensure all links use relative paths
- Check for syntax errors in markdown

### Local preview not working?

- Ensure Ruby and Bundler are installed
- Run `bundle install` to install dependencies
- Check for port conflicts (Jekyll uses port 4000 by default)

### Theme not applying?

- Verify theme name in `_config.yml`
- Clear Jekyll cache: `bundle exec jekyll clean`
- Rebuild: `bundle exec jekyll serve`

## Contributing

When updating documentation:

1. Create a feature branch
2. Make your changes
3. Test locally with `bundle exec jekyll serve`
4. Submit a pull request
5. Verify the preview deployment

## Resources

- [Jekyll Documentation](https://jekyllrb.com/docs/)
- [GitHub Pages Documentation](https://docs.github.com/en/pages)
- [Markdown Guide](https://www.markdownguide.org/)
- [Cayman Theme](https://github.com/pages-themes/cayman)
