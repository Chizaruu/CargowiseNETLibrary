# GitHub Actions Workflows

This directory contains GitHub Actions workflows for automated CI/CD.

## Workflows

### Build, Test, and Publish (`build-and-publish.yml`)

**Triggers**:

- Push to `main` or `develop` branches
- Pull requests to `main` branch
- Manual workflow dispatch

**Jobs**:

1. **build-and-test** - Builds the library, runs tests, and performs basic validation

   - Builds with .NET 8.0
   - Runs tests (if test project exists)
   - Uploads code coverage to Codecov
   - Verifies generated models exist
   - Ensures no XSD schemas are committed

2. **code-quality** - Analyzes code quality

   - Runs `dotnet format` to check formatting
   - Checks for TODO/FIXME comments
   - Ensures code style consistency

3. **publish** - Versions and publishes packages (main branch only)

   - Uses GitVersion for semantic versioning
   - Publishes to GitHub Packages
   - Publishes to NuGet.org (if API key configured)
   - Creates GitHub releases with detailed notes
   - Includes legal disclaimers in release notes

4. **notify-success** - Provides build summary

## Versioning Strategy

Uses **GitVersion** with the following strategy:

- **main branch**: Continuous Delivery, patch increment (1.0.0, 1.0.1, 1.0.2...)
- **develop branch**: Continuous Deployment with `alpha` tag (1.1.0-alpha.1)
- **feature branches**: Inherit increment with `feat` tag (1.1.0-feat.my-feature.1)
- **hotfix branches**: Patch increment with `beta` tag (1.0.2-beta.1)
- **release branches**: Beta releases (1.1.0-beta.1)

### Version Bump Messages

Control version increments with commit messages:

```bash
# Major version bump (1.0.0 -> 2.0.0)
git commit -m "Breaking change +semver:major"

# Minor version bump (1.0.0 -> 1.1.0)
git commit -m "Add new feature +semver:minor"

# Patch version bump (1.0.0 -> 1.0.1)
git commit -m "Fix bug +semver:patch"

# No version bump
git commit -m "Update docs +semver:none"
```

## Setup Instructions

### 1. Required Secrets

Configure these secrets in repository settings:

#### Optional Secrets

- **NUGET_API_KEY** (optional) - NuGet.org API key for publishing

  - Get from https://www.nuget.org/account/apikeys
  - Not required if only using GitHub Packages

- **CODECOV_TOKEN** (optional) - Codecov upload token for code coverage
  - Get from https://codecov.io/
  - Only needed if you want code coverage reports

#### Auto-configured Secrets

- **GITHUB_TOKEN** - Automatically provided by GitHub Actions
  - No configuration needed

### 2. Enable GitHub Packages and Security Features

1. Go to repository Settings
2. Navigate to Actions → General
3. Under "Workflow permissions", select "Read and write permissions"
4. Check "Allow GitHub Actions to create and approve pull requests"
5. Save changes

**Required Permissions** (automatically configured in workflow):

- `contents: write` - For creating releases and tags
- `packages: write` - For publishing to GitHub Packages
- `security-events: write` - For uploading security scan results

### 3. Enable Code Scanning (Optional)

To publish to NuGet.org:

1. Create NuGet API key at https://www.nuget.org/account/apikeys

   - Set key name: "GitHub Actions - CargoWiseNetLibrary"
   - Select scopes: "Push new packages and package versions"
   - Select packages: "CargoWiseNetLibrary" (or all packages)
   - Expiration: Choose appropriate duration

2. Add secret to GitHub repository:
   - Go to Settings → Secrets and variables → Actions
   - Click "New repository secret"
   - Name: `NUGET_API_KEY`
   - Value: [paste your NuGet API key]

### 4. Manual Publishing

To manually trigger a build and publish:

1. Go to Actions tab
2. Select "Build, Test, and Publish CargoWiseNetLibrary"
3. Click "Run workflow"
4. Select branch (typically `main`)
5. Check "Publish to NuGet.org" if desired
6. Click "Run workflow"

## Package Locations

### GitHub Packages

After successful publish to `main`:

```
https://github.com/Chizaruu/CargoWiseNetLibrary/packages
```

Install from GitHub Packages:

```bash
# Add GitHub Packages source
dotnet nuget add source https://nuget.pkg.github.com/Chizaruu/index.json \
  -n github \
  -u USERNAME \
  -p GITHUB_TOKEN

# Install package
dotnet add package CargoWiseNetLibrary --source github
```

### NuGet.org

After successful publish to `main` (if API key configured):

```
https://www.nuget.org/packages/CargoWiseNetLibrary
```

Install from NuGet.org:

```bash
dotnet add package CargoWiseNetLibrary
```

## Workflow Status Badges

Add to README.md:

```markdown
[![Build Status](https://github.com/Chizaruu/CargoWiseNetLibrary/actions/workflows/build-and-publish.yml/badge.svg)](https://github.com/Chizaruu/CargoWiseNetLibrary/actions/workflows/build-and-publish.yml)
[![NuGet](https://img.shields.io/nuget/v/CargoWiseNetLibrary.svg)](https://www.nuget.org/packages/CargoWiseNetLibrary/)
[![codecov](https://codecov.io/gh/Chizaruu/CargoWiseNetLibrary/branch/main/graph/badge.svg)](https://codecov.io/gh/Chizaruu/CargoWiseNetLibrary)
```

## Troubleshooting

### Build Fails: "No XSD schemas should be committed"

**Solution**: XSD schemas are proprietary and should not be committed. They're .gitignore'd. This check ensures they haven't been accidentally committed.

### Tests Don't Run

**Solution**: The workflow checks for test projects and only runs tests if `CargoWiseNetLibrary.Tests` exists. This is normal for new repositories.

### NuGet Publish Skipped

**Solution**: Publishing to NuGet.org only happens if `NUGET_API_KEY` secret is configured. Publishing to GitHub Packages always happens on `main` branch pushes.

### Version Not Incrementing

**Solution**:

1. Ensure you're on the `main` branch
2. Check GitVersion.yml configuration
3. Use semantic versioning commit messages (e.g., `+semver:minor`)
4. Verify tags are created properly

### Coverage Upload Fails

**Solution**: Codecov upload is optional and set to not fail CI. If you want coverage reports:

1. Sign up at https://codecov.io/
2. Add repository
3. Add `CODECOV_TOKEN` secret

## Maintenance

### Updating Workflow

When updating the workflow:

1. Test changes in a feature branch first
2. Create PR to `develop` branch
3. Verify workflow runs successfully
4. Merge to `main` when stable

### Updating Dependencies

Keep GitHub Actions up to date:

```bash
# Check for newer versions
# - actions/checkout@v4
# - actions/setup-dotnet@v4
# - actions/cache@v3
# - codecov/codecov-action@v4
# - gittools/actions/gitversion/*@v0.10.2
# - softprops/action-gh-release@v1
```

## Related Documentation

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [GitVersion Documentation](https://gitversion.net/docs/)
- [NuGet Publishing Guide](https://docs.microsoft.com/en-us/nuget/nuget-org/publish-a-package)
- [GitHub Packages Documentation](https://docs.github.com/en/packages)

---

**Questions?** Open an issue or contact contact@koalahollow.com
