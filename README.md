# Bien Oblige

## Overview

**Bien Oblige** is an [GNU Affero GPL](/LICENSE) licensed task management system designed to optimize task tracking and execution across multiple applications, helping teams navigate their tasks with greater insight and visibility.

## Status: MVP Implementation Phase

This project is currently in the MVP implementation phase. In this stage, we are actively developing the product and preparing for the initial release in Q1 2025.

### Vision

Our [Internal Press Release](docs/press/Internal%20Press%20Release.md) provides a high-level overview of the project's vision and objectives.

### Phase Objectives

* Core Feature Development
  * Implement the essential features that fulfill the primary use cases identified during the planning phase.
  * Ensure these features align with user needs and deliver immediate value to early adopters upon MVP release.
* Architecture and Infrastructure Setup
  * Establish the foundational architecture and infrastructure to support the MVP features.
  * Ensure the system is scalable and maintainable for future development phases.
  * Identify sample implementations that may be used as reference points for future development.
* API Development and Integration
  * Develop and test APIs for core functionalities, ensuring they meet performance and security standards.
  * Create a Client API tool for .NET developers to interact with the system. This tool will be the primary means of interacting with the system for the MVP, and thus should be engineered for ease of use and future expansion.
* Testing and Quality Assurance
  * Establish a testing framework to ensure all components of the MVP are rigorously tested.
  * Identify sample implementations that may be used as reference points for future development.
* Documentation and Support
  * Create comprehensive user and technical documentation to aid users and developers in understanding and using the MVP.
  * Leverage support channels on GitHub and Mastodon to gather feedback and address issues quickly.

### Contribution

In this phase, contributions are still welcome in the form of feedback, suggestions, and insights related to requirements and possible implementation strategies.

Additionally, we are open to collaboration opportunities with those interested in contributing to the project in the areas of documentation as well as the creation of Target Objects that represent the different types of entities that tasks can be performed on. See [Car](./src/BienOblige.Api/Targets/Car.cs) and [Residence](./src/BienOblige.Api/Targets/Residence.cs) as examples.

You can contact our primary via [Mastodon](https://fosstodon.org/@bsstahl).

Please note that since the project is still in the MVP stage, some changes might occur as we refine our understanding and approach.

## Key Concepts

Please see our [Ubiquitous Language](docs/ul.md) document for a detailed overview of the key concepts and entities in the **Bien Oblige** system.

## Features

Please see our [Features](./docs/features.md) document for a detailed overview of the key features and functionalities of the **Bien Oblige** system.

## Our Name

The project name **Bien Oblige** ("Well Obligated") reflects its core mission of supporting and serving users efficiently, much like the phrase's connotation of duty and helpfulness. This name was chosen to encapsulate the project's intent to facilitate task management with a sense of reliability and readiness to assist, aligning with its open-source nature and goal of fostering collaboration and support across different applications. The name embodies the spirit of being well-obliged to provide a seamless and effective task management experience.

## Respectful Community

To maintain an open and respectful dialog among the community, we adhere to a strict accountability policy. This policy outlines our expectations regarding intolerance and discrimination, ensuring a safe and welcoming environment for all contributors. For more information, please refer to our [Strict Accountability Policy](docs/strict-accountability.md).

## License

**Bien Oblige** is released under the GNU Affero General Public License v3.0. See [LICENSE](LICENSE.md) for details.

## Contact

For questions or support, please contact [Barry Stahl](https://fosstodon.org/@Bsstahl).
