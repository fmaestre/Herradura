using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xtrategics.DentiCenterLib.BaseClasses
{
    public enum enmCustomError
    {
        no_error_has_occurred = 0,
        rec_appointment_sequence_is_null = 1,
        rec_dateFromOrTo_is_null = 2,
        primaryKey_not_found = 3,
        object_not_changed = 4,
        object_not_initialized = 5,
        object_mustbe_new_to_search = 6,
        no_criteria_to_search = 7,
        object_mustbe_saved_first = 8,
        no_object_description = 9,
        missing_phone = 10,
        email_or_email_type_not_valid = 11,
        No_Data_Found = 12,
        Item_Object_Already_Exists = 13,
        Object_Already_Saved = 14,
        FirstName_LastName_CanNotBeEmpty = 15,
        The_Parent_Patient_MustBe_Titular = 16,
        The_RelatedPatient_MustBe_Dependant = 17,
        MissingInfo_InRelatedPatient = 18,
        The_RelatedPatient_AlreadyExists = 19,
        DependantPatient_IsLinked_To_A_DiffTitular = 20,
        InsuranceType_ClaimType_CanNotBe_Null = 21,
        DependantPatient_MustHave_A_Titular_Subscriber = 22,
        The_Patient_MustBe_Dependant = 23,
        The_Titular_CanNotBe_Dependant_OrHave_Parents = 24,
        The_RelatedPatient_MustBe_Titular = 25,
        DependantPatient_MustHave_OnlyOneTitular = 26,
        TitularPatient_CanNotBeSaved_ThruDependants = 27,
        UCRPatient_MustHave_A_Titular_Subscriber = 28,
        UCRPatient_MustHave_OnlyOneTitular = 29,
        PK_CanNotChange_WhenTheObjectIsSaved = 30,
        PatientClaimTypeCanNotBeChangeInThisScreen = 31,
        Invalid_PatientClaimType = 32,
        PatientTypeCanNotBeChangeInThisScreen = 33,
        Invalid_PatientType = 34,
        rec_clinicId_cannot_be_empty = 35,
        insuransePlanGroup_must_be_empty_to_delete = 36,
        patient_has_dependants = 37,
        patient_has_treatments = 38,
        patient_has_appointments = 39,
        Dependant_Must_be_deleted_first = 40,



        FirstName_LastName_UserName_Can_not_be_Empty=41,
        Account_DoesNot_Exist = 42,
        Record_was_migrated_cannot_be_deleted = 43,
        input_parameter_was_not_supplied = 44,
        medService_is_not_allowed_in_this_clinic = 45,
        medService_is_not_associated_to_insurance_or_pln_or_group = 46,
        medService_is_not_allowed_for_insurance_or_pln_or_group = 47,
        there_was_not_information_that_invalidate_this_med_service = 48,
        there_is_missed_info = 49
    }
}
