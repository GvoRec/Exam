import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder} from '@angular/forms';
import {concat, Observable, of, Subject} from "rxjs";
import {catchError, debounceTime, distinctUntilChanged, filter, map, switchMap, tap} from "rxjs/operators";

import {DocumentModel} from "../models/documentModel";
import {EmployeeModel} from "../models/employeeModel";
import {BackendService} from "../services/backend.service";
import {saveAs} from 'file-saver';
import {OrganizationModel} from "../models/organizationModel";

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html'
})
export class DocumentsComponent implements OnInit {
  minLengthTerm = 3;


  //ng select organization
  organizations$: Observable<OrganizationModel>;
  loading = false;
  organizationsInput$ = new Subject<string>();
  selectedOrganization: OrganizationModel = null;

  //ng select employee
  employees: EmployeeModel[];
  selectedEmployee: EmployeeModel = null;

  //ng select document
  documents$: Observable<DocumentModel>;
  documentsInput$ = new Subject<string>();
  selectedDocument: DocumentModel = null;

  documentForm = this.formBuilder.group({
    name: '',
    placeholders: ['']
  });

  generateDocumentForm = this.formBuilder.group({
    documentId: '',
    employeeId: '',
    placeholdersValue: ''
  });
  private baseUrl: string;
  private backendService: BackendService;

  constructor(private formBuilder: FormBuilder, backendService: BackendService,
              @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = `${baseUrl}api`;
    this.backendService = backendService;
  }

  ngOnInit() {
    this.loadOrganizations();
    // this.loadEmployees();
    this.loadDocuments();
  }

  public loadOrganizations() {
    this.organizations$ = this.observableShit<OrganizationModel>(this.organizationsInput$, 'organization');
  }

  public loadDocuments() {
    this.documents$ = this.observableShit<DocumentModel>(this.documentsInput$, 'documents');
  }

  public getEmployees(): Observable<any> {
    return this.backendService.getEmployees(this.selectedOrganization.id.toString(), '');
  }

  public buildDocumentPlaceholdersWithValues(employee: EmployeeModel, document: DocumentModel): void {
    if (employee !== null && document !== null) {
      let documentPlaceholders = document.placeholders;
      let filledPairs = [];

      for (let documentPlaceholder of documentPlaceholders) {
        let keyValuePair = `${documentPlaceholder}:<value>`
        for (let personField in employee) {
          if (personField.toUpperCase() === documentPlaceholder.toUpperCase()) {
            keyValuePair = `${documentPlaceholder}:${employee[personField]}`
          }
        }
        filledPairs.push(keyValuePair);
      }
      let keyValuePairs = filledPairs.join(";");

      this.generateDocumentForm = this.formBuilder.group({
        documentId: document.id,
        placeholdersValue: keyValuePairs
      });
    }
  }

  public onSubmit(): void {
    let name = this.documentForm.get('name').value;
    let placeholderValue = <string>(this.documentForm.get('placeholders').value);
    let placeholders = placeholderValue.split(",");
    this.backendService.addDocument({name: name, placeholders: placeholders}).subscribe(res => {
      this.backendService.getDocuments().subscribe(res => {
        console.log('документ добавлен');
      });
    });
    this.documentForm.reset();
  }

  public generateDocument(): void {
    this.backendService.generateFile(this.selectedDocument.id, this.generateDocumentForm.get('placeholdersValue').value)
      .subscribe(
        success => {
          this.generateDocumentForm.reset();
          this.selectedOrganization = null;
          this.selectedEmployee = null;
          let documentName = this.selectedDocument.name;
          this.selectedDocument = null;
          saveAs(success, documentName);

        },
        err => {
          alert("Server error while downloading file.");
        }
      );
  }

  public trackByFn(item: any) {
    return item.id;
  }

  public valueSelected($event) {
    this.buildDocumentPlaceholdersWithValues(this.selectedEmployee, this.selectedDocument);
  }

  public organizationSelected() {
    this.backendService.getEmployees(this.selectedOrganization.id.toString()).subscribe(res =>
      this.employees = res);
  }

  private observableShit<T>(inputString: Subject<string>, method: string): Observable<any> {
    return concat(
      of([]), // default items
      inputString.pipe(
        filter(res => {
          return res !== null && res.length >= this.minLengthTerm
        }),
        distinctUntilChanged(),
        debounceTime(300),
        tap(() => this.loading = true),
        switchMap(term => {
          return this.backendService.choiceMethod(method, term).pipe(
            catchError(() => of([])), // empty list on error
            tap(() => {
              this.loading = false;
            })
          )
        })
      )
    );
  }
}

