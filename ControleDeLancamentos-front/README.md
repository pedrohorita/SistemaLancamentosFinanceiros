# Financial Transactions App

This project is a front-end application that allows users to manage financial transactions. Users can add, edit, list, and delete transactions through a user-friendly interface.

## Features

- **Add Transactions**: Users can input details for new financial transactions.
- **Edit Transactions**: Users can modify existing transactions.
- **List Transactions**: A comprehensive list of all transactions is displayed.
- **Delete Transactions**: Users can remove transactions they no longer need.

## Project Structure

```
financial-transactions-app
├── public
│   ├── index.html          # Main HTML file
│   └── favicon.ico         # Application favicon
├── src
│   ├── components          # React components for the application
│   │   ├── AddTransactionForm.tsx
│   │   ├── EditTransactionForm.tsx
│   │   ├── TransactionList.tsx
│   │   └── TransactionItem.tsx
│   ├── pages               # Pages of the application
│   │   ├── HomePage.tsx
│   │   └── TransactionsPage.tsx
│   ├── services            # API service for transactions
│   │   └── api.ts
│   ├── styles              # CSS styles
│   │   └── styles.css
│   ├── App.tsx             # Main application component
│   ├── index.tsx           # Entry point for the React application
│   └── types               # TypeScript types
│       └── transaction.ts
├── package.json            # npm configuration
├── tsconfig.json           # TypeScript configuration
└── README.md               # Project documentation
```

## Getting Started

1. Clone the repository:
   ```
   git clone <repository-url>
   ```

2. Navigate to the project directory:
   ```
   cd ControleDeLancamentos-front
   ```

3. Install dependencies:
   ```
   npm install
   ```

4. Start the development server:
   ```
   npm start
   ```

## Technologies Used

- React
- TypeScript
- CSS
- Docker (for containerization, if applicable)

## License

This project is licensed under the MIT License.