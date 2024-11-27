# Use Case: Annual Preventive Maintenance (PM) Procedure for MetroTransit Bus Type X

**Objective**: To ensure the timely and efficient completion of the annual preventive maintenance for a specific bus type, enhancing operational reliability and safety and ensuring compliance with all regulations and company policies.

For a description of the MetroTransit system and actors, see the [MetroTransit Use Case Overview](./README.md).

The monthly PM procedure for MetroTransit buses involves a series of tasks that must be completed in a specific order to ensure the bus is safe and operational. The tasks are broken down into phases, each with its own set of responsibilities and requirements. The completion of these tasks is critical to maintaining the safety and reliability of the bus fleet. In certain cases, the work can be performed by either the mechanics or the lot attendants, while other tasks can only be performed by a mechanic and still other only by a supervisor.

## Task Hierarchy Creation

* **Identify Major Maintenance Phases**:
  * **Inspection Phase**: Initial assessment of the bus condition.
  * **Repair Phase**: Address any issues found during inspection.
  * **Testing Phase**: Ensure all repairs are successful and the bus meets safety standards.
  * **Documentation Phase**: Record all work completed and update maintenance logs.
  * **Analysis Phase**: Review data to identify areas for improvement.

2. **Break Down Each Phase into Tasks**:
    * **Inspection Phase**:
        * Check engine components.
        * Inspect brake systems.
        * Assess tire wear and pressure.
    * **Repair Phase**:
        * Replace worn-out brake pads.
        * Fix engine oil leaks.
        * Align wheels.
    * **Testing Phase**:
        * Conduct brake efficiency test.
        * Perform emission testing.
        * Verify operational controls.
    * **Documentation Phase**:
        * Update maintenance records.
        * Report to supervisors.
        * Schedule the next PM session.


3. **Assign Tasks to Roles**:
    * **Lot Attendants**: Responsible for initial inspections and assisting with part replacements.
    * **Mechanics**: Handle complex repairs and testing.
    * **Supervisors**: Oversee the process, verify documentation, and approve task completion.
    * **Analysts**: Analyze task data to identify bottlenecks and inefficiencies.


---


## User Interaction with the Task Management System


1. **Lot Attendants**:
    * **Platform**: Mobile Devices
    * **Features**: Mobile app for inspection checklists, task status updates, and notifications.
    * **Usage**: Scan bus IDs to access task lists, mark inspections as complete, and flag issues.


2. **Mechanics**:
    * **Platform**: Windows Applications on Terminals
    * **Features**: Access maintenance history, log repairs, and schedule tasks.
    * **Usage**: Address issues flagged by attendants, input repair details, and trigger testing tasks.


3. **Supervisors**:
    * **Platform**: Desktop Browser
    * **Features**: Web-based dashboard for task management, analytics, and approval processes.
    * **Usage**: Monitor task progress, verify completion, and generate reports.


4. **Analysts**:
    * **Platform**: Analytical Tools
    * **Features**: Custom queries, scripts, and reports for data analysis.
    * **Usage**: Identify bottlenecks, analyze inefficiencies, and provide process improvement insights.


---


## Integration Considerations


* **Data Synchronization**:
    * Achieve near real-time updates using event-driven processes.
    * Implement alerts and reminders based on events.


* **User Access Management**:
    * Define role-based access and permissions.
    * Ensure secure authentication mechanisms.


* **User Experience Optimization**:
    * Tailor interfaces to user needs and environments.
    * Incorporate feedback for continuous improvement.


* **Data Flow and Accessibility for Analysts**:
    * Ensure structured data flows via events.
    * Support integration with analytical tools.


* **Security and Data Governance**:
    * Maintain robust data governance and access controls.