# Comprehensive Development and Bug Fix Plan

## Introduction
This document outlines the new, comprehensive plan to address all reported bugs, performance issues, and missing features. The previous phased approach is deprecated. The new strategy is to work module-by-module, ensuring each is fully functional and feature-complete before proceeding.

---

## Phase 0: Foundational Fixes (Highest Priority)
*These issues affect the entire application and must be resolved first.*

1.  **Fix DataGrid Loading & Performance:**
    - **Issue:** Data is not appearing in grids after being added. Menus load slowly.
    - **Action:** Investigate the data loading pipeline (`MainWindowViewModel` -> `...ListViewModel` -> `...Manager`). Ensure the `ObservableCollection` is being updated on the UI thread and that async calls are not blocking. Add logging to `try-catch` blocks to expose any suppressed errors. This is the absolute first priority.

2.  **Fix Theme Logic & Appearance:**
    - **Issue:** Theme starts dark, switches to light, doesn't save correctly, and the dark color is too harsh.
    - **Action:**
        - Fix the logic in `App.axaml.cs` to correctly load the saved theme from `config.txt` **once** on startup. The default will be "Light".
        - Fix the `SettingsViewModel` to correctly reflect the current state and only save the theme when the user actively changes it.
        - Adjust the `FluentTheme` dark mode background color to a softer, more appropriate shade of dark gray.

---

## Phase 1: Müşteriler (Customers) Module - Complete Overhaul
*Goal: Make this module fully functional and feature-rich as per the specification.*

1.  **Fix `NOT NULL` Crash (Validation):**
    - **Issue:** App crashes when saving a customer with an empty required field (e.g., `AdSoyad`).
    - **Action:** Implement client-side validation in `CustomerAddEditViewModel`. Before saving, check for empty required fields. If invalid, prevent saving and trigger a UI warning (e.g., making the label red for 2 seconds).

2.  **Implement Full UI:**
    - **Issue:** The "Add/Edit" screen is missing most fields.
    - **Action:** Update `CustomerAddEditView.axaml` to include UI controls for all fields in the `Musteri` model, especially a button and display area for `MusteriFotograf`.

3.  **Implement Full Features:**
    - **Delete Button:** Add a "Delete Customer" button to the `CustomerListView`.
    - **Status Logic:**
        - Create and apply an `IValueConverter` to translate status values (`active`, `archived`) into Turkish for display in the `DataGrid`.
        - Modify `CustomerRepository.GetAllAsync` to add a `WHERE Status != 'deleted'` clause.
    - **Detailed Search Window:**
        - Create a new `CustomerSearchView.axaml` and `CustomerSearchViewModel.cs` based on the user's provided code example.
        - Add a "Detailed Search" button to `CustomerListView` that opens the new view as a dialog window.
        - The search functionality will query all fields and will include an option to show "deleted" customers.

---

## Phase 2: Stok (Stock) Module - Complete Overhaul

1.  **Validation & UI:**
    - Implement validation in `StockAddEditViewModel` to prevent crashes on required fields.
    - Update `StockAddEditView.axaml` to have UI controls for all fields in the `Stok` model.
2.  **Delete Button:** Add and implement the "Delete" button on the `StockListView`.

---

## Phase 3: Bildirimler (Notifications) Module - Complete Overhaul

1.  **Fix `InvalidCastException`:**
    - **Issue:** Crash when interacting with the `DatePicker`.
    - **Action:** Implement a custom `IValueConverter` to reliably convert between the `DateTimeOffset?` required by the control and the `DateTime?` used in the model/database.
2.  **Implement Advanced UI:**
    - Rebuild the `BildirimAddEditView.axaml` to include the advanced UI for conditional alerts.
    - Implement the ViewModel logic (`IsZamanlanmisVisible`, etc.) to dynamically show/hide UI sections based on the selected notification `Tip`.

---

## Phase 4: Muhasebe (Accounting) Module - Complete Overhaul

1.  **Fix Bugs & Add Time Picker:**
    - Address any remaining `NOT NULL` validation issues.
    - Research and implement a `TimePicker` control to be used alongside the `DatePicker` for transaction times.
2.  **Implement Full UI (from Specification):**
    - Rebuild `AccountingAddEditView.axaml` to match the detailed user specification.
    - Add checkboxes to enable/disable KDV and Commission.
    - Add a "Net/Brüt" selection mechanism.
    - Add a button for invoice photo upload and an `Image` control to display it.
3.  **Implement Calculator Input:**
    - Create a new `CalculatorView.axaml` dialog window, using the user's C# code as a direct reference for the logic and UI.
    - Wire this dialog to appear on a double-click event on the main amount `TextBox`.
4.  **Implement Backend Logic:**
    - Update `AccountingAddEditViewModel` to manage the state of all new UI controls.
    - Implement the calculation logic for KDV, Commission, and the Net/Brüt conversions.
    - Implement the warning pop-up when changing from Net to Brüt with active taxes/fees.

---

## Phase 5: Final Verification
- After all modules are complete, a final `dotnet build` will be performed.
- A detailed verification message will be sent, outlining the checks that will be performed for each new feature and bug fix before submitting.
