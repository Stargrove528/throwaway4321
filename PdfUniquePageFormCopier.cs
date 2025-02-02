
// Type: com.digitalarcsystems.Traveller.utility.PdfUniquePageFormCopier




using Common.Logging;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility
{
  public class PdfUniquePageFormCopier : IPdfPageExtraCopier
  {
    private static ILog logger = LogManager.GetLogger(typeof (PdfUniquePageFormCopier));
    private PdfAcroForm formFrom;
    private PdfAcroForm formTo;
    private PdfDocument documentFrom;
    private PdfDocument documentTo;

    public virtual void Copy(PdfPage fromPage, PdfPage toPage)
    {
      if (this.documentFrom != fromPage.GetDocument())
      {
        this.documentFrom = fromPage.GetDocument();
        this.formFrom = PdfAcroForm.GetAcroForm(this.documentFrom, false);
      }
      if (this.documentTo != toPage.GetDocument())
      {
        this.documentTo = toPage.GetDocument();
        this.formTo = PdfAcroForm.GetAcroForm(this.documentTo, true);
      }
      if (this.formFrom == null)
        return;
      IList<PdfName> excludeKeys = (IList<PdfName>) new List<PdfName>();
      excludeKeys.Add(PdfName.Fields);
      excludeKeys.Add(PdfName.DR);
      this.formTo.GetPdfObject().MergeDifferent(this.formFrom.GetPdfObject().CopyTo(this.documentTo, excludeKeys, false));
      IDictionary<string, PdfFormField> formFields1 = this.formFrom.GetFormFields();
      if (formFields1.Count <= 0)
        return;
      IDictionary<string, PdfFormField> formFields2 = this.formTo.GetFormFields();
      foreach (PdfAnnotation annotation in (IEnumerable<PdfAnnotation>) toPage.GetAnnotations())
      {
        if (annotation.GetSubtype().Equals((object) PdfName.Widget))
          this.CopyField(toPage, formFields1, formFields2, annotation);
      }
    }

    private PdfFormField MakeFormField(PdfObject fieldDict)
    {
      PdfFormField pdfFormField = PdfFormField.MakeFormField(fieldDict, this.documentTo);
      if (pdfFormField == null)
        PdfUniquePageFormCopier.logger.Warn((object) MessageFormatUtil.Format("Cannot create form field from a given PDF object: {0}", (object) fieldDict.GetIndirectReference()));
      return pdfFormField;
    }

    private void CopyField(
      PdfPage toPage,
      IDictionary<string, PdfFormField> fieldsFrom,
      IDictionary<string, PdfFormField> fieldsTo,
      PdfAnnotation currentAnnot)
    {
      PdfDictionary asDictionary = currentAnnot.GetPdfObject().GetAsDictionary(PdfName.Parent);
      if (asDictionary != null)
      {
        PdfFormField parentField = PdfUniquePageFormCopier.GetParentField(asDictionary, this.documentTo);
        if (parentField == null || parentField.GetFieldName() == null)
          return;
        this.CopyParentFormField(toPage, fieldsTo, currentAnnot, parentField);
      }
      else
      {
        PdfString asString = currentAnnot.GetPdfObject().GetAsString(PdfName.T);
        string key = (string) null;
        if (asString != null)
          key = asString.ToUnicodeString();
        if (key == null || !fieldsFrom.ContainsKey(key))
          return;
        PdfFormField pdfFormField = this.MakeFormField((PdfObject) currentAnnot.GetPdfObject());
        if (pdfFormField == null)
          return;
        if (fieldsTo.Keys.Contains(key))
          pdfFormField = this.MergeFieldsWithTheSameName(pdfFormField);
        this.formTo.AddField(pdfFormField, toPage);
      }
    }

    private void CopyParentFormField(
      PdfPage toPage,
      IDictionary<string, PdfFormField> fieldsTo,
      PdfAnnotation annot,
      PdfFormField parentField)
    {
      PdfString fieldName1 = parentField.GetFieldName();
      if (!fieldsTo.ContainsKey(fieldName1.ToUnicodeString()))
      {
        PdfFormField parentFieldCopy = this.CreateParentFieldCopy(annot.GetPdfObject(), this.documentTo);
        PdfArray kids = parentFieldCopy.GetKids();
        parentFieldCopy.GetPdfObject().Remove(PdfName.Kids);
        this.formTo.AddField(parentFieldCopy, toPage);
        parentFieldCopy.GetPdfObject().Put(PdfName.Kids, (PdfObject) kids);
      }
      else
      {
        PdfFormField newField = this.MakeFormField((PdfObject) annot.GetPdfObject());
        if (newField == null)
          return;
        PdfString fieldName2 = newField.GetFieldName();
        if (fieldName2 != null)
        {
          if (fieldsTo.Get<string, PdfFormField>(fieldName2.ToUnicodeString()) != null)
          {
            PdfFormField pdfFormField = this.MergeFieldsWithTheSameName(newField);
            this.formTo.GetFormFields().Put<string, PdfFormField>(pdfFormField.GetFieldName().ToUnicodeString(), pdfFormField);
          }
          else
          {
            HashSet<string> existingFields = new HashSet<string>();
            this.GetAllFieldNames(this.GetFields(this.formTo), (ICollection<string>) existingFields);
            this.AddChildToExistingParent(annot.GetPdfObject(), (ICollection<string>) existingFields, fieldsTo);
          }
        }
        else
        {
          if (parentField.GetKids().Contains((PdfObject) newField.GetPdfObject()))
            return;
          HashSet<string> existingFields = new HashSet<string>();
          this.GetAllFieldNames(this.GetFields(this.formTo), (ICollection<string>) existingFields);
          this.AddChildToExistingParent(annot.GetPdfObject(), (ICollection<string>) existingFields);
        }
      }
    }

    private PdfFormField MergeFieldsWithTheSameName(PdfFormField newField)
    {
      string unicodeString = newField.GetFieldName().ToUnicodeString();
      PdfString asString = newField.GetPdfObject().GetAsString(PdfName.T);
      PdfUniquePageFormCopier.logger.Warn((object) MessageFormatUtil.Format("The document already has field {0}. Annotations of the fields with this name will be added to the existing one as children. If you want to have separate fields, please, rename them manually before copying.", (object) unicodeString));
      PdfFormField field = this.formTo.GetField(unicodeString);
      if (field.IsFlushed())
      {
        int num = 0;
        do
        {
          ++num;
          newField.SetFieldName(asString.ToUnicodeString() + "_#" + num.ToString());
        }
        while (this.formTo.GetField(newField.GetFieldName().ToUnicodeString()) != null);
        return newField;
      }
      newField.GetPdfObject().Remove(PdfName.T);
      newField.GetPdfObject().Remove(PdfName.P);
      this.GetFields(this.formTo).Remove((PdfObject) field.GetPdfObject());
      PdfArray kids1 = field.GetKids();
      if (kids1 != null && !kids1.IsEmpty())
      {
        field.AddKid(newField);
        return field;
      }
      field.GetPdfObject().Remove(PdfName.T);
      field.GetPdfObject().Remove(PdfName.P);
      PdfFormField emptyField = PdfFormField.CreateEmptyField(this.documentTo);
      emptyField.Put(PdfName.FT, (PdfObject) field.GetFormType()).Put(PdfName.T, (PdfObject) asString);
      PdfDictionary parent = field.GetParent();
      if (parent != null)
      {
        emptyField.Put(PdfName.Parent, (PdfObject) parent);
        PdfArray asArray = parent.GetAsArray(PdfName.Kids);
        for (int index = 0; index < asArray.Size(); ++index)
        {
          if (asArray.Get(index) == field.GetPdfObject())
          {
            asArray.Set(index, (PdfObject) emptyField.GetPdfObject());
            break;
          }
        }
      }
      PdfArray kids2 = field.GetKids();
      if (kids2 != null)
        emptyField.Put(PdfName.Kids, (PdfObject) kids2);
      emptyField.AddKid(field).AddKid(newField);
      if (field.GetValue() != null)
        emptyField.Put(PdfName.V, field.GetPdfObject().Get(PdfName.V));
      return emptyField;
    }

    private static PdfFormField GetParentField(PdfDictionary parent, PdfDocument pdfDoc)
    {
      PdfDictionary asDictionary = parent.GetAsDictionary(PdfName.Parent);
      return asDictionary != null ? PdfUniquePageFormCopier.GetParentField(asDictionary, pdfDoc) : PdfFormField.MakeFormField((PdfObject) parent, pdfDoc);
    }

    private PdfFormField CreateParentFieldCopy(PdfDictionary fieldDic, PdfDocument pdfDoc)
    {
      PdfDictionary asDictionary = fieldDic.GetAsDictionary(PdfName.Parent);
      PdfFormField parentFieldCopy = PdfFormField.MakeFormField((PdfObject) fieldDic, pdfDoc);
      if (asDictionary != null)
      {
        parentFieldCopy = this.CreateParentFieldCopy(asDictionary, pdfDoc);
        PdfArray pdfArray = (PdfArray) asDictionary.Get(PdfName.Kids);
        if (pdfArray == null)
          asDictionary.Put(PdfName.Kids, (PdfObject) new PdfArray((PdfObject) fieldDic));
        else
          pdfArray.Add((PdfObject) fieldDic);
      }
      return parentFieldCopy;
    }

    private void AddChildToExistingParent(
      PdfDictionary fieldDic,
      ICollection<string> existingFields)
    {
      PdfDictionary asDictionary = fieldDic.GetAsDictionary(PdfName.Parent);
      if (asDictionary == null)
        return;
      PdfString asString = asDictionary.GetAsString(PdfName.T);
      if (asString == null)
        return;
      string unicodeString = asString.ToUnicodeString();
      if (existingFields.Contains(unicodeString))
      {
        asDictionary.GetAsArray(PdfName.Kids).Add((PdfObject) fieldDic);
      }
      else
      {
        asDictionary.Put(PdfName.Kids, (PdfObject) new PdfArray((PdfObject) fieldDic));
        this.AddChildToExistingParent(asDictionary, existingFields);
      }
    }

    private void AddChildToExistingParent(
      PdfDictionary fieldDic,
      ICollection<string> existingFields,
      IDictionary<string, PdfFormField> fieldsTo)
    {
      PdfDictionary asDictionary = fieldDic.GetAsDictionary(PdfName.Parent);
      if (asDictionary == null)
        return;
      PdfString asString = asDictionary.GetAsString(PdfName.T);
      if (asString == null)
        return;
      string unicodeString = asString.ToUnicodeString();
      if (existingFields.Contains(unicodeString))
      {
        PdfArray asArray = asDictionary.GetAsArray(PdfName.Kids);
        foreach (PdfObject fieldDict in asArray)
        {
          if (((PdfDictionary) fieldDict).Get(PdfName.T).Equals((object) fieldDic.Get(PdfName.T)))
          {
            PdfFormField pdfFormField1 = this.MakeFormField(fieldDict);
            PdfFormField newField = this.MakeFormField((PdfObject) fieldDic);
            if (pdfFormField1 != null && newField != null)
            {
              fieldsTo.Put<string, PdfFormField>(pdfFormField1.GetFieldName().ToUnicodeString(), pdfFormField1);
              PdfFormField pdfFormField2 = this.MergeFieldsWithTheSameName(newField);
              this.formTo.GetFormFields().Put<string, PdfFormField>(pdfFormField2.GetFieldName().ToUnicodeString(), pdfFormField2);
              return;
            }
          }
        }
        asArray.Add((PdfObject) fieldDic);
      }
      else
      {
        asDictionary.Put(PdfName.Kids, (PdfObject) new PdfArray((PdfObject) fieldDic));
        this.AddChildToExistingParent(asDictionary, existingFields);
      }
    }

    private void GetAllFieldNames(PdfArray fields, ICollection<string> existingFields)
    {
      foreach (PdfObject field in fields)
      {
        if (!field.IsFlushed())
        {
          PdfDictionary pdfDictionary = (PdfDictionary) field;
          PdfString asString = pdfDictionary.GetAsString(PdfName.T);
          if (asString != null)
            existingFields.Add(asString.ToUnicodeString());
          PdfArray asArray = pdfDictionary.GetAsArray(PdfName.Kids);
          if (asArray != null)
            this.GetAllFieldNames(asArray, existingFields);
        }
      }
    }

    protected internal virtual PdfArray GetFields(PdfAcroForm acroForm)
    {
      PdfArray fields = acroForm.GetPdfObject().GetAsArray(PdfName.Fields);
      if (fields == null)
      {
        fields = new PdfArray();
        acroForm.GetPdfObject().Put(PdfName.Fields, (PdfObject) fields);
      }
      return fields;
    }
  }
}
