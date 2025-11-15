# Legal Information and Disclaimers

**Document Version**: 1.1  
**Last Updated**: November 16, 2025  
**Effective Date**: January 1, 2025

---

## ⚖️ Important Legal Notices

### Critical Summary

**BEFORE USING THIS LIBRARY, YOU MUST UNDERSTAND**:

1. ⚠️ **CargoWise® Authorization Required** - You MUST be an authorized CargoWise User or Affiliated Partner
2. ✅ **Maintainer is Authorized** - The library maintainer (Chizaruu) is an authorized CargoWise user with legal schema access
3. ⚠️ **Models Don't Grant Access** - Having these C# models does NOT authorize you to use CargoWise systems
4. ✅ **Apache 2.0 Code License** - The library code and infrastructure are Apache 2.0 licensed
5. ⚠️ **You Are Responsible** - You must ensure you have proper CargoWise authorization before using this library

---

## Schema Ownership and Intellectual Property

### Trademark Notices

**CargoWise®** and **WiseTech Global®** are registered trademarks of **WiseTech Global Limited**, an Australian company.

**Important**: All use of these trademarks in this repository is for identification and reference purposes only. No trademark license is granted by this repository.

### XSD Schema Ownership

1. **Exclusive Ownership**: All CargoWise XSD schemas, data structures, XML namespaces, and related intellectual property are the exclusive property of WiseTech Global Limited and/or its licensors.

2. **Proprietary Rights**: The schemas define proprietary data structures and business logic that remain the intellectual property of WiseTech Global Limited.

3. **No Schema Distribution**: This repository does NOT distribute CargoWise XSD schema files. The schemas are `.gitignore`'d and must be obtained directly from CargoWise by authorized users.

### Generated Model Classes

1. **Model Origin**: The C# model classes in this library (`Universal.cs` and `Native.cs`) have been generated from CargoWise XSD schemas using automated code generation tools.

2. **Generation Process**:

   - **Tool Used**: [Chizaruu.NetTools.SchemaGenerator](https://github.com/Chizaruu/Chizaruu.NetTools)
   - **Underlying Engine**: XmlSchemaClassGenerator v2.1.1183.0
   - **Method**: Automated transformation from XSD to C#
   - **Legal Headers**: All generated files include legal notices about authorization requirements

3. **Maintainer Authorization**:

   - The library maintainer (Chizaruu) is an **authorized CargoWise user**
   - Schema access obtained through legitimate CargoWise application **EDI Messaging** functionality
   - Schemas accessed and used in accordance with CargoWise user agreement terms
   - All model generation performed on properly licensed systems

4. **Derived Work Considerations**:
   - Generated models are machine-transformed representations of schema structures
   - Models maintain structural relationships defined in original schemas
   - WiseTech Global retains all rights to underlying schema designs and structures
   - This repository provides the models as a development convenience for authorized users

### Your Authorization Requirements

**CRITICAL UNDERSTANDING**:

✅ **What This Library Provides**:

- Pre-generated C# model classes
- Serialization and validation utilities
- Helper methods for common tasks
- Development convenience for authorized users

⚠️ **What This Library Does NOT Provide**:

- Authorization to use CargoWise systems
- License to access CargoWise APIs
- Permission to process CargoWise data without proper agreements
- Right to use CargoWise trademarks
- Exemption from WiseTech Global's terms of service

**YOU MUST**:

1. ✅ Be an authorized CargoWise User OR CargoWise Affiliated Partner
2. ✅ Comply with ALL terms of your CargoWise user agreement
3. ✅ Have a valid, active CargoWise license or partnership agreement
4. ✅ Respect WiseTech Global's intellectual property rights
5. ✅ Use the library only for authorized CargoWise integration purposes

**YOU MUST NOT**:

1. ❌ Use this library to circumvent CargoWise authorization requirements
2. ❌ Access CargoWise systems without proper authorization
3. ❌ Redistribute CargoWise XSD schemas (they're not included anyway)
4. ❌ Claim this library grants you CargoWise access rights
5. ❌ Use the library for unauthorized reverse engineering or competitive purposes

---

## License Information

### This Library's Code and Infrastructure

The **original code, utilities, and infrastructure** of CargoWiseNetLibrary are licensed under the **Apache License 2.0**.

**Copyright 2025 Chizaruu (Abdul-Kadir Coskun)**

```
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this library except in compliance with the License.
You may obtain a copy of the License at:

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```

**Apache 2.0 License Scope**:

The Apache 2.0 license applies to:

- ✅ C# serialization utilities (`XmlSerializer<T>`, `JsonSerializer<T>`)
- ✅ Validation framework (`ModelValidator<T>`, `ValidationResult`)
- ✅ Helper utilities (`UniversalInterchangeHelper`, extension methods)
- ✅ Project infrastructure and documentation
- ✅ Generated C# model classes (as derived works, with restrictions noted below)

### Generated Models and Schema-Derived Content

**IMPORTANT CLARIFICATION**:

1. **License vs Authorization**:

   - The **code** is Apache 2.0 licensed (you can use, modify, distribute the code)
   - **BUT** using the models requires **CargoWise authorization** (separate requirement)
   - Think of it like: Open-source driver for proprietary hardware - driver is open, hardware access requires license

2. **What You Can Do With Apache 2.0 License**:

   - ✅ Use the library in your projects (commercial or non-commercial)
   - ✅ Modify the code to suit your needs
   - ✅ Distribute the library
   - ✅ Include in proprietary software
   - ✅ Use the generated models if you're an authorized CargoWise user

3. **Additional Requirements Beyond Apache 2.0**:
   - ⚠️ You MUST be an authorized CargoWise user to use the models
   - ⚠️ You MUST comply with your CargoWise user agreement
   - ⚠️ You MUST respect WiseTech Global's intellectual property rights
   - ⚠️ Apache 2.0 does NOT grant CargoWise system access

### Dual-Requirement Summary

```
┌─────────────────────────────────────────────────────────────┐
│ TO USE THIS LIBRARY, YOU NEED BOTH:                        │
├─────────────────────────────────────────────────────────────┤
│ 1. Apache 2.0 Compliance                                   │
│    ✅ Follow open-source license terms                     │
│    ✅ Include copyright notices                            │
│    ✅ Note modifications if you make any                   │
│                                                             │
│ 2. CargoWise Authorization (SEPARATE REQUIREMENT)          │
│    ⚠️ Be an authorized CargoWise User or Partner          │
│    ⚠️ Comply with CargoWise user agreement                │
│    ⚠️ Have valid CargoWise license/partnership            │
└─────────────────────────────────────────────────────────────┘
```

### How to Obtain CargoWise Authorization

**If you don't have CargoWise authorization**:

1. **Contact WiseTech Global**:

   - Visit: https://www.cargowise.com/
   - Request information about becoming a CargoWise user or affiliated partner

2. **Contact Your Company's CargoWise Administrator**:

   - If your company uses CargoWise, request access through internal channels

3. **Access XSD Schemas** (once authorized):
   - Log into CargoWise application
   - Navigate to EDI Messaging functionality
   - Export/download XSD schemas
   - Place in `schemas/` directory
   - Regenerate models if needed

**XSD schemas are available ONLY to authorized users through the CargoWise application**.

---

## Redistribution and Derivative Works

### Redistribution Rights

Under Apache 2.0, you may redistribute this library, **BUT**:

1. ✅ **You May**:

   - Fork this repository
   - Include in your own projects
   - Redistribute modified or unmodified versions
   - Create packages or distributions

2. ⚠️ **You Must**:

   - Include the Apache 2.0 LICENSE file
   - Include this LEGAL.md file or a prominent link to it
   - Maintain all legal notices in generated model files
   - Make clear that CargoWise authorization is required
   - Include attribution to original author (Chizaruu / Koala Hollow)

3. ❌ **You Must Not**:
   - Remove or modify legal headers from generated files
   - Imply the library grants CargoWise authorization
   - Remove authorization requirement notices
   - Redistribute CargoWise XSD schemas (they're not included anyway)
   - Claim WiseTech Global endorsement

### Derivative Works

If you create derivative works:

1. **You Must**:

   - Retain all legal notices and disclaimers
   - Clearly mark your modifications
   - Not remove CargoWise authorization requirements
   - Comply with Apache 2.0 attribution requirements

2. **Consider**:
   - Contributing improvements back to this project
   - Maintaining compatibility with upstream changes
   - Documenting your changes clearly

---

## Authorization Revocation and Compliance

### If You Lose CargoWise Authorization

**IMPORTANT**: If your CargoWise authorization is revoked, terminated, or expires:

1. ⚠️ **You MUST immediately cease** using this library with CargoWise systems
2. ⚠️ **You MUST NOT continue** accessing CargoWise APIs or data
3. ✅ **You MAY continue** to use the library code itself (it's Apache 2.0)
4. ⚠️ **You MUST NOT use** the generated models for CargoWise integration
5. ✅ **You MAY study** the code for educational purposes

**Your Responsibilities**:

- Monitor your CargoWise authorization status
- Ensure continued compliance with CargoWise agreements
- Cease use immediately if authorization ends
- Delete any CargoWise data you no longer have rights to process

### Compliance Verification

Users are responsible for:

1. ✅ Maintaining valid CargoWise authorization
2. ✅ Regularly reviewing CargoWise user agreement terms
3. ✅ Ensuring all team members using the library are authorized
4. ✅ Implementing appropriate access controls
5. ✅ Documenting authorization status in your systems

---

## Removal Request Policy

### For WiseTech Global Limited

**WiseTech Global Limited** or any authorized representative may request removal of this library or any content from this repository.

**Process**:

1. **Send a formal request** to: contact@koalahollow.com

   - Subject line: "CargoWiseNetLibrary - Legal Notice"
   - Include clear identification of content in question
   - Provide verification of authority to make the request

2. **We commit to**:

   - ✅ Respond within **48 business hours**
   - ✅ Promptly remove the entire repository OR specific content as requested
   - ✅ Cooperate fully with your legal team
   - ✅ Provide confirmation of removal

3. **We will NOT**:
   - ❌ Argue or delay legitimate requests
   - ❌ Re-upload removed content
   - ❌ Encourage others to fork before removal

**We respect WiseTech Global's intellectual property rights** and will act swiftly on any legitimate concerns.

### For Other Rights Holders

If you believe any content in this repository infringes your intellectual property rights, please contact contact@koalahollow.com with:

- Description of copyrighted work
- Location of allegedly infringing content
- Your contact information
- Good faith statement of unauthorized use

---

## User Responsibilities

### General Responsibilities

**YOU ARE RESPONSIBLE FOR**:

1. ✅ **Authorization**: Being an authorized CargoWise User or Affiliated Partner
2. ✅ **Compliance**: Following all terms of your CargoWise user agreement
3. ✅ **Legal Compliance**: Ensuring compliance with applicable laws and regulations
4. ✅ **IP Respect**: Respecting WiseTech Global's intellectual property rights
5. ✅ **Data Security**: Protecting CargoWise data and credentials
6. ✅ **Team Authorization**: Ensuring all users on your team are authorized
7. ✅ **Monitoring**: Monitoring your authorization status
8. ✅ **Data Protection**: Complying with GDPR, CCPA, and other privacy laws

### Development and Deployment Responsibilities

When using this library in development or production:

1. **Development**:

   - ✅ Ensure development team members are authorized
   - ✅ Protect test data appropriately
   - ✅ Don't commit credentials to version control
   - ✅ Use appropriate access controls

2. **Production**:

   - ✅ Validate authorization before deployment
   - ✅ Implement proper security measures
   - ✅ Monitor for security vulnerabilities
   - ✅ Have incident response procedures

3. **Third-Party Services**:
   - ✅ Ensure service providers are authorized
   - ✅ Have appropriate data processing agreements
   - ✅ Verify compliance with terms

---

## Disclaimers

### No Warranty

**THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND**, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.

**IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY**, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

### Specific Disclaimers

1. **No Accuracy Guarantee**:

   - Models are machine-generated and may contain errors
   - Verify all data before production use
   - Test thoroughly in development environments

2. **No Maintenance Guarantee**:

   - No guarantee of updates or bug fixes
   - CargoWise schema changes may require regeneration
   - Community-maintained project

3. **No Support Guarantee**:
   - No official support provided
   - Community support via GitHub issues
   - No SLA or response time guarantees

### No Affiliation

This project is **NOT**:

- ❌ Affiliated with, endorsed by, or sponsored by WiseTech Global Limited
- ❌ An official WiseTech Global or CargoWise product
- ❌ Supported or maintained by WiseTech Global
- ❌ A replacement for official CargoWise integration tools
- ❌ A grant of license to use CargoWise systems
- ❌ Authorized to use WiseTech Global trademarks beyond fair use

This is an **independent, community-driven open-source project** created by an authorized CargoWise user (Chizaruu / Koala Hollow) for the benefit of other authorized users.

### No Legal Advice

Nothing in this repository constitutes legal advice. Consult with qualified legal counsel regarding:

- Your CargoWise license obligations
- Intellectual property rights
- Data protection and privacy compliance
- Software licensing questions
- Any other legal matters

### No Authorization Grant

**CRITICAL UNDERSTANDING**:

```
┌─────────────────────────────────────────────────────────┐
│ THIS LIBRARY DOES NOT GRANT CARGOWISE AUTHORIZATION    │
├─────────────────────────────────────────────────────────┤
│ ❌ Models don't authorize CargoWise access             │
│ ❌ Maintainer's authorization doesn't transfer to you  │
│ ❌ Apache 2.0 license doesn't grant CargoWise rights  │
│ ✅ You need your OWN CargoWise authorization          │
│ ✅ You are responsible for your compliance            │
└─────────────────────────────────────────────────────────┘
```

---

## Data Protection and Privacy

### GDPR and Data Protection Compliance

If you use this library to process personal data:

1. **Data Controller**: You are the data controller for any personal data you process
2. **Legal Basis**: You must have proper legal basis for processing (consent, contract, etc.)
3. **Data Subject Rights**: You must honor data subject rights (access, deletion, etc.)
4. **Data Protection Officer**: Consider whether you need to appoint a DPO
5. **Impact Assessments**: Conduct DPIAs where required

**Applicable Regulations**:

- 🇪🇺 GDPR (General Data Protection Regulation)
- 🇺🇸 CCPA (California Consumer Privacy Act)
- 🇬🇧 UK GDPR
- 🌍 Other applicable data protection laws in your jurisdiction

### Security Responsibilities

**You are responsible for**:

1. **Credential Security**:

   - Securing API credentials and access tokens
   - Using environment variables or secure vaults
   - Not committing secrets to version control
   - Rotating credentials regularly

2. **Data Security**:

   - Encrypting sensitive data in transit (TLS/SSL)
   - Encrypting sensitive data at rest
   - Implementing appropriate access controls
   - Monitoring for unauthorized access

3. **Vulnerability Management**:

   - Monitoring for security vulnerabilities
   - Applying security updates promptly
   - Conducting security assessments
   - Having incident response procedures

4. **Compliance**:
   - Meeting security requirements in your jurisdiction
   - Following industry best practices
   - Complying with CargoWise security requirements
   - Maintaining audit logs where required

### Data Retention and Deletion

When you process CargoWise data:

1. **Retention Policies**:

   - Define appropriate retention periods
   - Comply with legal retention requirements
   - Delete data when no longer needed

2. **Data Deletion**:
   - Implement secure deletion procedures
   - Honor data subject deletion requests
   - Maintain deletion records where required

---

## Export Control and International Use

### Export Compliance

Users are responsible for compliance with:

1. **U.S. Export Controls**:

   - Export Administration Regulations (EAR)
   - Not distributing to embargoed countries
   - Screening against denied parties lists

2. **International Trade Laws**:
   - Compliance with local export/import regulations
   - Sanctions and embargo compliance
   - Technology transfer restrictions

### International Use

When using this library internationally:

1. **Data Localization**:

   - Comply with data localization requirements
   - Understand cross-border data transfer rules
   - Implement appropriate safeguards (SCCs, BCRs, etc.)

2. **Local Laws**:
   - Comply with local software licensing laws
   - Respect local intellectual property rights
   - Follow local data protection regulations

---

## Trademark Notice

### WiseTech Global Trademarks

The following are trademarks or registered trademarks of **WiseTech Global Limited**:

- **CargoWise®**
- **WiseTech Global®**
- Other product and service names associated with WiseTech Global

**Use of Trademarks**:

- All use in this repository is for **identification and reference purposes only**
- No trademark license is granted
- Fair use for descriptive purposes
- No implication of endorsement or sponsorship

### Other Trademarks

All other trademarks mentioned in this repository are the property of their respective owners.

---

## Liability Limitations

### Limited Liability

**TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW**:

1. **No Consequential Damages**: The authors and copyright holders shall not be liable for any indirect, incidental, special, consequential, or punitive damages.

2. **No Business Loss**: Not liable for loss of profits, revenue, business opportunities, data, or goodwill.

3. **Maximum Liability**: Total liability shall not exceed the amount paid for the library (which is $0 for open-source users).

### Indemnification

**You agree to indemnify and hold harmless** the library authors, contributors, and copyright holders from:

- Claims arising from your use of the library
- Your violation of these terms
- Your violation of CargoWise user agreements
- Your violation of third-party rights

---

## Modifications to This Notice

### Change Policy

We reserve the right to modify this legal notice at any time.

**When changes occur**:

1. ✅ Document version number will be incremented
2. ✅ "Last Updated" date will be changed
3. ✅ Significant changes will be noted in git commit messages
4. ✅ Users should review periodically

**Continued use** of the library after modifications constitutes acceptance of the updated terms.

**Current Version**: 1.1  
**Last Updated**: November 16, 2025

### Notification

Significant legal changes will be announced via:

- GitHub releases
- Repository README updates
- Git commit messages

---

## Contact Information

### Legal Matters

**For Legal Notices, Takedown Requests, or Concerns**:

- **Email**: contact@koalahollow.com
- **Subject Line**: "CargoWiseNetLibrary - Legal Notice"
- **Response Time**: Within 48 business hours

### Technical Support

**For Technical Questions**:

- **GitHub Issues**: https://github.com/Chizaruu/CargoWiseNetLibrary/issues
- **Discussions**: https://github.com/Chizaruu/CargoWiseNetLibrary/discussions

### General Inquiries

**For Other Questions**:

- **Email**: contact@koalahollow.com
- **Subject Line**: "CargoWiseNetLibrary - Inquiry"

---

## Summary for WiseTech Global Representatives

**To WiseTech Global Legal Team**:

We understand and respect your intellectual property rights. Regarding this library:

### Our Commitments

✅ **Authorized Access**: The maintainer (Chizaruu) is an authorized CargoWise user with legitimate schema access via EDI Messaging

✅ **Clear Requirements**: Users are clearly and repeatedly informed they must be authorized CargoWise Users or Affiliated Partners

✅ **No Workaround**: This library is NOT intended to circumvent your authorization requirements - it's a development tool for authorized users

✅ **No Schema Distribution**: We do NOT distribute your proprietary XSD schemas - they're .gitignore'd

✅ **Legal Notices**: All generated models include prominent legal notices about authorization requirements

✅ **Cooperation**: We will cooperate fully with any concerns or requests

✅ **Rapid Response**: We commit to responding to legal requests within 48 business hours

✅ **Takedown Policy**: We will promptly remove content or the entire repository as requested

### Our Intent

The purpose of this library is to:

- Provide development convenience for authorized CargoWise users and partners
- Offer helper utilities for serialization and validation
- Support the CargoWise developer community
- **NOT** to circumvent your authorization requirements
- **NOT** to compete with your official tools
- **NOT** to violate your intellectual property rights

### Contact Us

**If you have any concerns, questions, or requests**:

- Email: contact@koalahollow.com
- Subject: "CargoWiseNetLibrary - WiseTech Global Legal"
- We will respond promptly and professionally

We value your intellectual property and are committed to operating within proper legal boundaries.

---

## For Users - Final Summary

**Before using this library, ensure you understand:**

### ✅ What You Need

1. **Technical Requirements**:

   - .NET 8.0 or higher
   - Understanding of C# development
   - Knowledge of XML/JSON serialization

2. **Legal Requirements**:
   - Valid CargoWise authorization (User or Affiliated Partner)
   - Active CargoWise license or partnership agreement
   - Compliance with your CargoWise user agreement
   - Understanding of your data protection obligations

### ⚠️ Critical Understanding

```
┌─────────────────────────────────────────────────────┐
│ KEY POINTS TO UNDERSTAND                            │
├─────────────────────────────────────────────────────┤
│ 1. Library code is Apache 2.0 (open source)       │
│ 2. BUT you need CargoWise authorization to use it  │
│ 3. Models don't grant CargoWise access rights     │
│ 4. You must comply with CargoWise agreements      │
│ 5. You're responsible for your authorization      │
│ 6. Maintainer's auth doesn't transfer to you      │
└─────────────────────────────────────────────────────┘
```

### 📚 Resources

- **Main Documentation**: [README.md](README.md)
- **Quick Start**: [QUICKSTART.md](QUICKSTART.md)
- **Schema Generation**: [SCHEMA_GENERATION.md](SCHEMA_GENERATION.md)
- **CargoWise Info**: https://www.cargowise.com/
- **Tool Repository**: https://github.com/Chizaruu/Chizaruu.NetTools

### ❓ Common Questions

**Q: Can I use this library without CargoWise authorization?**  
A: No. You must be an authorized CargoWise User or Affiliated Partner.

**Q: Does the Apache 2.0 license grant me CargoWise access?**  
A: No. The Apache 2.0 license covers the library code, but you still need separate CargoWise authorization.

**Q: Can I redistribute this library?**  
A: Yes, under Apache 2.0 terms, but recipients also need CargoWise authorization to use it.

**Q: What if I lose my CargoWise authorization?**  
A: You must immediately cease using the library for CargoWise integration.

**Q: Can I modify the generated models?**  
A: Yes, but it's recommended to use partial classes or extension methods instead to preserve regeneration capability.

**Q: Is this library supported by WiseTech Global?**  
A: No. This is an independent community project, not affiliated with WiseTech Global.

---

## Acknowledgments

**This library respects and acknowledges**:

- **WiseTech Global Limited** for creating CargoWise and maintaining comprehensive EDI schemas
- **XmlSchemaClassGenerator** (Michael Ganss) for the underlying code generation engine
- **The CargoWise Community** for supporting logistics technology innovation
- **All Contributors** who help improve this library

---

**Thank you for respecting intellectual property rights and using this library responsibly.**

---

**Document Version**: 1.1  
**Effective Date**: January 1, 2025  
**Last Updated**: November 16, 2025  
**Maintained By**: Chizaruu
