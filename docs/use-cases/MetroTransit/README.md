# Detailed Use Cases

## MetroTransit

MetroTransit is a fictional public transportation company that operates a large fleet of buses. To ensure the safety and reliability of its vehicles, MetroTransit has implemented a comprehensive maintenance system that leverages Bien Oblige for task management. This system manages the maintenance tasks for the entire bus fleet, from initial inspections to complex repairs and regulatory certifications. The system involves multiple user roles, each with specific responsibilities and interfaces tailored to their needs.

## Actors

* **Lot Attendants** - Conduct initial inspections, perform minor repairs, and update task statuses using mobile devices.
* **Mechanics** - Address complex repairs, perform testing of systems and components, certify regulatory inspections, and update the task management system with repair details and task statuses using dedicated terminals.
* **Supervisors** - Oversee the entire maintenance process, verify task completion, approve documentation, monitor progress through a web-based dashboard, and generate reports for future planning.
* **Analysts** - Utilize data flowing into their systems to monitor task completion times, identify bottlenecks and inefficiencies, and provide insights and recommendations for process optimization using customized queries, scripts, and reports.

## The Bus Maintenance Systems

Each user community interacts with the system through dedicated applications tailored to their needs.

* Lot Attendants utilize the *LotCheck* Mobile app on their handheld devices to perform initial inspections and update task statuses, interfacing with the system via a Mobile Backend-for-Frontend (BFF) service.
* Mechanics access the system through the *MechConnect* Terminal application on dedicated Windows terminals througout the garage, allowing them to handle detailed repair tasks and record updates, facilitated by a Windows BFF.
* Supervisors manage and oversee the entire maintenance process using the *SuperVisor* Web application accessed via desktop browsers, which connects through a Web BFF service for comprehensive task management and reporting.

Central to the system, Bien Oblige operates as the core task management hub ensuring seamless data flow and coordination among the various user roles. Task updates and information flow bidirectionally between the apps and the system, while the Data Warehouse receives data events for storage and analysis. Analysts leverage this data through their analytical tools to identify process inefficiencies and recommend improvements, enhancing the overall efficiency and reliability of the maintenance operations.

```mermaid
graph LR
    A((Lot Attendants)) --> |Uses| App1[LotCheck Mobile]
    App1 <--> |Tasks &<br>Task Updates| BFF1[Mobile BFF] <--> |Tasks &<br>Task Updates| B{{Bien Oblige}}

    C((Mechanics)) --> |Uses| App2[MechConnect Terminal]
    App2 <--> |Tasks &<br>Task Updates| BFF2[Windows BFF] <--> |Tasks &<br>Task Updates| B

    D((Supervisors)) --> |Uses| App3[SuperVisor Web]
    App3 <--> |Tasks &<br>Task Updates| BFF3[Web BFF] <--> |Tasks &<br>Task Updates| B

    B --> |Data Events| F[(Data Warehouse)]
    F --> |Feeds| E((Analysts))
```

## Use Case Analyses

* [Bus Nightly Procedures](./bus-nightly-procedures.md)
* [Bus Annual PM Procedure](./bus-annual-pm-procedure.md)
