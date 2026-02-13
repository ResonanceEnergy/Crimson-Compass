# Crimson Compass Production Deployment Guide

## Overview

This guide covers the complete production deployment pipeline for Crimson Compass, from development to production release and ongoing maintenance.

## Quick Start

### Automated Full Deployment
```powershell
# Complete production deployment (validation, build, deploy)
.\deploy.ps1 -Target Both

# Deploy to app stores
.\deploy-to-stores.ps1 -Target Both -Environment production

# Monitor post-launch metrics
.\monitor-launch.ps1 -SendAlerts
```

### Version Management
```powershell
# Bump version (patch, minor, major, build)
.\bump-version.ps1 -BumpType minor -Environment production

# View current version
Get-Content version.json | ConvertFrom-Json
```

## Pipeline Overview

### Phase 1: Development & Validation
1. **Code Validation**: Episode data and audio validation
2. **Unit Testing**: Unity Test Framework execution
3. **Integration Testing**: Episode loading and compass mechanics
4. **Performance Testing**: Memory usage and frame rate validation

### Phase 2: Build & Package
1. **Cross-Platform Builds**: Android APK and iOS IPA generation
2. **Code Signing**: Automatic certificate management
3. **Artifact Management**: Organized build outputs with metadata
4. **Size Optimization**: Build size analysis and reporting

### Phase 3: Deployment & Distribution
1. **Staging Deployment**: Internal testing and QA
2. **Beta Testing**: External tester distribution
3. **Production Release**: App store submission and release
4. **Rollback Procedures**: Emergency version rollback capabilities

### Phase 4: Monitoring & Maintenance
1. **Crash Reporting**: Real-time crash monitoring and alerting
2. **Performance Monitoring**: API response times and app performance
3. **User Analytics**: Engagement metrics and retention tracking
4. **Automated Alerts**: Slack notifications for critical issues

## Prerequisites

### Development Environment
- Unity 6000.3.6f1 or later
- PowerShell 7.0 or later
- Python 3.9 or later
- Git LFS for large assets

### App Store Accounts
- Google Play Console account with app created
- Apple Developer Program account with app created
- Fastlane installed (`gem install fastlane`)

### CI/CD Setup (Optional but Recommended)
- GitHub repository with Actions enabled
- Service account keys for app stores
- Slack webhook for notifications

## Environment Configuration

### Version Management
The `version.json` file controls all version information:

```json
{
  "version": "1.0.0",
  "build": 1,
  "androidVersionCode": 1,
  "iosBuildNumber": "1.0.0.1",
  "environments": {
    "development": { "apiEndpoint": "https://dev-api.crimsoncompass.com" },
    "staging": { "apiEndpoint": "https://staging-api.crimsoncompass.com" },
    "production": { "apiEndpoint": "https://api.crimsoncompass.com" }
  }
}
```

### Environment Variables
Set these for automated deployments:

```powershell
# Google Play Store
$env:GOOGLE_PLAY_JSON_KEY_PATH = "path/to/service-account.json"

# Apple App Store
$env:APP_STORE_CONNECT_API_KEY = "path/to/api-key.p8"
$env:APP_STORE_ISSUER_ID = "your-issuer-id"

# Slack Notifications
$env:SLACK_WEBHOOK_URL = "https://hooks.slack.com/..."
```

## Deployment Workflows

### Development Workflow
```powershell
# 1. Make changes and test locally
# 2. Run validation
python Tools/validate_episode_data.py
python Tools/validate_audio_events.py

# 3. Bump version for development
.\bump-version.ps1 -BumpType build -Environment development -NoCommit

# 4. Build and test
.\deploy.ps1 -Target Android -Clean
```

### Staging Deployment
```powershell
# 1. Bump version
.\bump-version.ps1 -BumpType minor -Environment staging

# 2. Full deployment to staging
.\deploy.ps1 -Target Both -Clean

# 3. Deploy to beta channels
.\deploy-to-stores.ps1 -Target Both -Environment staging
```

### Production Deployment
```powershell
# 1. Final validation
.\deploy.ps1 -Target Both -Clean

# 2. Bump version for production
.\bump-version.ps1 -BumpType minor -Environment production

# 3. Deploy to production app stores
.\deploy-to-stores.ps1 -Target Both -Environment production

# 4. Start monitoring
.\monitor-launch.ps1 -SendAlerts
```

## CI/CD Integration

### GitHub Actions Setup
The `.github/workflows/production-deploy.yml` provides automated CI/CD:

- **Triggers**: Push to main/develop, manual dispatch
- **Validation**: Automated testing and validation
- **Builds**: Cross-platform Unity builds
- **Deployment**: Automated app store submission
- **Notifications**: Slack alerts for deployment status

### Required Secrets
Set these in your GitHub repository secrets:

```
ANDROID_KEYSTORE_BASE64     # Base64 encoded keystore
ANDROID_KEYSTORE_PASS       # Keystore password
ANDROID_KEYALIAS_NAME       # Key alias name
ANDROID_KEYALIAS_PASS       # Key alias password
IOS_TEAM_ID                 # Apple Developer Team ID
IOS_DEVELOPER_ID            # Apple Developer ID
IOS_DEVELOPER_PASSWORD      # Apple Developer password
SLACK_WEBHOOK_URL           # Slack webhook for notifications
```

## Monitoring & Alerting

### Automated Monitoring
The `monitor-launch.ps1` script provides comprehensive monitoring:

- **Crash Rates**: Alerts if crash rate exceeds 2% (warning) or 5% (critical)
- **API Performance**: Monitors response times and error rates
- **User Engagement**: Tracks DAU, session duration, retention
- **App Store Ratings**: Monitors rating changes

### Alert Thresholds
```
Critical Alerts (> immediate action required):
- Crash Rate: > 5%
- API Error Rate: > 10%
- Response Time: > 5 seconds

Warning Alerts (> monitor closely):
- Crash Rate: > 2%
- API Error Rate: > 5%
- Response Time: > 2 seconds
```

### Manual Monitoring
Access monitoring dashboards:
- Firebase Crashlytics: Crash reporting and analysis
- Google Analytics: User engagement and retention
- App Store Connect: Download and revenue metrics
- Google Play Console: Installs and ratings

## Rollback Procedures

### Emergency Rollback
```powershell
# Rollback to previous version
.\rollback.ps1 -TargetVersion "1.0.0" -Environment production

# Verify rollback
.\monitor-launch.ps1
```

### Partial Rollback
For iOS/Android specific issues:
```powershell
# Rollback only Android
.\rollback.ps1 -TargetVersion "1.0.0" -Environment production -Platform Android
```

## Troubleshooting

### Common Issues

#### Build Failures
```powershell
# Clean and rebuild
.\deploy.ps1 -Target Both -Clean

# Check Unity logs
Get-Content Logs/UnityBuild.log -Tail 50
```

#### Validation Failures
```powershell
# Run individual validators
python Tools/validate_episode_data.py
python Tools/validate_audio_events.py

# Check validation logs
Get-Content validation-results.json | ConvertFrom-Json
```

#### App Store Rejections
1. Check submission logs in `Builds/deployment_report_*.json`
2. Review app store guidelines
3. Update metadata in Fastlane configuration
4. Resubmit with fixes

### Support Contacts
- **Technical Issues**: Development team
- **App Store Issues**: Store-specific support
- **Infrastructure**: DevOps team
- **User Issues**: Customer support

## Maintenance Schedule

### Daily
- Automated monitoring checks
- Crash report reviews
- Performance metric monitoring

### Weekly
- User engagement analysis
- App store rating monitoring
- Security vulnerability scans

### Monthly
- Comprehensive health check
- Performance optimization review
- Feature usage analysis

### Quarterly
- Major version updates
- Platform SDK updates
- Security audits

## Success Metrics

### Launch Metrics (First 24 hours)
- Crash Rate: < 2%
- App Store Rating: > 4.0
- User Retention: > 70% (Day 1)

### Ongoing Metrics
- Monthly Active Users: Target growth
- Average Session Duration: > 15 minutes
- Revenue per User: Target metrics
- App Store Rating: Maintain > 4.0

## Security Considerations

### Code Security
- Automated dependency scanning
- Code signing for all builds
- Obfuscation enabled for production builds

### Data Privacy
- GDPR and CCPA compliance
- Data encryption at rest and in transit
- User consent management

### Infrastructure Security
- Regular security updates
- Access control and monitoring
- Backup and disaster recovery procedures

---

## Quick Reference

### Commands
```powershell
# Full deployment
.\deploy.ps1 -Target Both

# Version bump
.\bump-version.ps1 -BumpType minor

# App store deployment
.\deploy-to-stores.ps1 -Target Both -Environment production

# Monitoring
.\monitor-launch.ps1 -SendAlerts
```

### File Locations
- `version.json`: Version configuration
- `Builds/`: Build artifacts
- `fastlane/`: App store deployment configuration
- `.github/workflows/`: CI/CD pipelines
- `docs/Production_Deployment_Plan.md`: Detailed planning document

For detailed planning information, see `docs/Production_Deployment_Plan.md`.</content>
<parameter name="filePath">c:\Users\gripa\OneDrive\Desktop\CrimsonCompass1\CrimsonCompass\DEPLOYMENT_README.md