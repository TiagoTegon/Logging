# C# Development Test
## Description
Create a logger class using the singleton design pattern in C#. The goal is to have only one object responsible for logging in the whole application. Consider some important features such a class might need, such as:

- [x] Asynchronous logging
- [x] Thread safety
- [x] Different logging levels
- [ ] and so on...

## Development
The developed class, called Logging, contains the following features:

- Six logging levels (NOTSET, DEBUG, INFO, WARNING, ERROR and CRITICAL);
- Singleton pattern ensuring that only one instance of the class can be created;
- lockObject is used to ensure mutual exclusion when creating a new instance of the class. It is used to avoid concurrency issues in a multithreaded environment;
- An inner class used to represent a log entry (logEntry). It contains properties for the message and the severity level of the log;
- It has a queue that is used to store the log messages that have been recorded;
- The logging task runs in the background and calls the LogMessagesAsync method repeatedly to process the messages in the log queue;
- LogMessagesAsync is the asynchronous method that is executed continuously by the logging task. It checks for messages in the log queue and, if there are, formats the message and writes it to the log file. If there are no messages, the method waits a short period before checking again;
- Shutdown method responsible for stopping the log recording. It cancels the token and waits for the logging task to complete;
- A method called FormatLogMessage responsible for formatting the log message with a timestamp and the severity level;
- At last the WriteLogMessage method responsible for writing the formatted log message to the console and saving it in a log file called "log.txt".
