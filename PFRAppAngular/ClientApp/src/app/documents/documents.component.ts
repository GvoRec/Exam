import {Component, Inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {FormBuilder} from '@angular/forms';
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {saveAs} from 'file-saver';
import {DocumentModel} from "../models/documentModel";
import {EmployeeModel} from "../models/employeeModel";

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html'
})
export class DocumentsComponent {
  public initialDocuments: DocumentModel[];
  public initialUsers: EmployeeModel[];
  public documents: DocumentModel[];
  public empoyees: EmployeeModel[];
  private baseUrl: string;
  private selectedUserIdValue: number;

  documentForm = this.formBuilder.group({
    name: '',
    placeholders: ['']
  });

  generateDocumentForm = this.formBuilder.group({
    documentId: '',
    employeeId: '',
    placeholdersValue: ''
  });

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private formBuilder: FormBuilder) {
    this.baseUrl = `${baseUrl}api`;
    http.get<DocumentModel[]>(this.baseUrl + '/document/list').subscribe(result => {
      this.initialDocuments = result;
      this.documents = this.initialDocuments;
    }, error => console.error(error));

    http.get<EmployeeModel[]>(this.baseUrl + '/employee/list/aa90eb65-8ecf-4e34-0417-08d9b8e339ca').subscribe(result => {
      this.initialUsers = result;
      this.empoyees = this.initialUsers;
    }, error => console.error(error));
  }

  public documentIdSelected(): boolean {
    return !!this.generateDocumentForm.get('documentId').value;
  }

  public setUserSelected(): void {
    this.selectedUserIdValue = <number>this.generateDocumentForm.get('employeeId').value;
    if(!!this.selectedUserIdValue){
      this.empoyees = this.initialUsers.filter(u => u.id == this.selectedUserIdValue);
    }else{
      this.generateDocumentForm.reset();
      this.empoyees = this.initialUsers;
    }

  }

  public filterDocuments(): void {
    let selectedDocumentIdValue = <number>this.generateDocumentForm.get('documentId').value;
    if (!!selectedDocumentIdValue) {
      this.documents = this.initialDocuments.filter(doc => doc.id == selectedDocumentIdValue)
      let document = this.documents[0];
      let keyValuePairs = "";
      let documentPlaceholders = document.placeholders;
      let selectedEmployee = this.empoyees.filter(e => e.id == this.selectedUserIdValue)[0];
      for (let p in documentPlaceholders) {
        let keyValuePair = `${document.placeholders[p]}:<value>;`
        for (let key in selectedEmployee)
        {
          let docPlaceholder = document.placeholders[p];
          if (docPlaceholder.toUpperCase() === key.toUpperCase()) {
            let value = selectedEmployee[key];
            keyValuePair = `${document.placeholders[p]}:${value};`
          }
        }
        keyValuePairs += keyValuePair;
      }
      this.generateDocumentForm = this.formBuilder.group({
        documentId: selectedDocumentIdValue,
        placeholdersValue: keyValuePairs
      });
    } else {
      this.generateDocumentForm.reset();
      this.documents = this.initialDocuments;
    }
  }

  onSubmit(): void {
    console.warn('Your order has been submitted', this.documentForm.value);
    let name = this.documentForm.get('name').value;
    let placeholderValue = <string>(this.documentForm.get('placeholders').value);
    let placeholders = placeholderValue.split(",");
    this.addDocument({name: name, placeholders: placeholders})
    this.documentForm.reset();
  }

  generateDocument(): void {
    let id = <number>this.generateDocumentForm.get('documentId').value;
    let fileName = this.initialDocuments.filter(doc => doc.id == id)[0].name;
    this.downloadFile(id)
      .subscribe(
        success => {
          saveAs(success, fileName);
        },
        err => {
          alert("Server error while downloading file.");
        }
      );
  }

  private addDocument(document: DocumentModel) {
    this.http.post<DocumentModel>(this.baseUrl + '/document/add', document).subscribe(res => {
      this.http.get<DocumentModel[]>(this.baseUrl + '/document/list').subscribe(result => {
        this.documents = result;
      }, error => console.error(error));
    });
  }

  private downloadFile(id: number): Observable<any> {
    let keyValuePairs = <string>this.generateDocumentForm.get('placeholdersValue').value;
    let url = this.baseUrl + `/document/generate/${id}?placeholdersValues=${keyValuePairs}`
    return this.http.get(url, {
      responseType: 'blob',
      observe: 'response'
    })
      .pipe(
        map((res: any) => {
          return new Blob([res.body], {type: 'docx'});
        })
      );

  }
}

