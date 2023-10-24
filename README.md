# Installations Monitor

Monitor of the installations made in a computer. It saves file system and registry changes to enable the user to purge the waste from uninstalled applications.

## Design

The application has four commands:

- `monitor`: starts the monitoring process.
- `installations`: shows the monitored installations.
- `installation`: shows the produced changes during a given installation.
- `remove`: removes the saved information for a given installation.

## Functioning

The application creates subscriptions to system events to be notified of changes in files system and registry. All these changes are saved in a context of the installation of one program in the computer. Then, the user can inspect the produced changes for a given program installation.